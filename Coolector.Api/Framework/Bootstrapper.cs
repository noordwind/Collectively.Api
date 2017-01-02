using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Autofac;
using Coolector.Api.Authentication;
using Coolector.Api.Validation;
using Coolector.Common.Extensions;
using Coolector.Common.Exceptionless;
using Coolector.Common.Nancy;
using Coolector.Common.Nancy.Serialization;
using Coolector.Common.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using NLog;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit.Configuration;
using ModuleContainer = Coolector.Api.IoC.ModuleContainer;
using StorageSettings = Coolector.Api.Storages.StorageSettings;
using Newtonsoft.Json;
using Coolector.Common.RabbitMq;

namespace Coolector.Api.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private static readonly string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string InvalidDecimalSeparator = DecimalSeparator == "." ? "," : ".";
        private readonly IConfiguration _configuration;

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#if DEBUG
        public override void Configure(INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(enabled: false, displayErrorTraces: true);
        }
#endif

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += (ctx) =>
            {
                FixNumberFormat(ctx);

                return null;
            };
            pipelines.AfterRequest += (ctx) => AddCorsHeaders(ctx.Response);
            SetupTokenAuthentication(container, pipelines);
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("Coolector API has started.");
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Logger.Info("Configuring IoC");
            base.ConfigureApplicationContainer(container);

            container.Update(builder =>
            {
                builder.RegisterType<CustomJsonSerializer>().As<JsonSerializer>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<AppSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<FeatureSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<JwtTokenSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<StorageSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
                builder.RegisterInstance(new MemoryCache(new MemoryCacheOptions())).As<IMemoryCache>().SingleInstance();
                builder.RegisterModule<ModuleContainer>();
                RabbitMqContainer.Register(builder, _configuration.GetSettings<RawRabbitConfiguration>());
            });
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                ctx.Response = ErrorResponse.FromException(ex, context.Environment);
                AddCorsHeaders(ctx.Response);
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "Coolector", "API");

                return ctx.Response;
            });
        }

        private void SetupTokenAuthentication(ILifetimeScope container, IPipelines pipelines)
        {
            var jwtTokenHandler = container.Resolve<IJwtTokenHandler>();
            var statelessAuthConfiguration =
                new StatelessAuthenticationConfiguration(ctx =>
                {
                    var token = jwtTokenHandler.GetFromAuthorizationHeader(ctx.Request.Headers.Authorization);
                    var isValid = jwtTokenHandler.IsValid(token);

                    return isValid ? new CoolectorIdentity(token.Sub) : null;
                });
            StatelessAuthentication.Enable(pipelines, statelessAuthConfiguration);
        }

        private void FixNumberFormat(NancyContext ctx)
        {
            if (ctx.Request.Query == null)
                return;

            var fixedNumbers = new Dictionary<string, double>();
            foreach (var key in ctx.Request.Query)
            {
                var value = ctx.Request.Query[key].ToString();
                if (!value.Contains(InvalidDecimalSeparator))
                    continue;

                var number = 0;
                if (int.TryParse(value.Split(InvalidDecimalSeparator[0])[0], out number))
                    fixedNumbers[key] = double.Parse(value.Replace(InvalidDecimalSeparator, DecimalSeparator));
            }
            foreach (var fixedNumber in fixedNumbers)
            {
                ctx.Request.Query[fixedNumber.Key] = fixedNumber.Value;
            }
        }

        private static void AddCorsHeaders(Response response)
        {
            response.WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE")
                .WithHeader("Access-Control-Allow-Headers",
                    "Authorization,Accept,Origin,Content-Type,User-Agent,X-Requested-With")
                .WithHeader("Access-Control-Expose-Headers", "X-Operation,X-Resource,Location");
        }
    }
}
