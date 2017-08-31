using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Autofac;
using Collectively.Api.Validation;
using Collectively.Common.Extensions;
using Collectively.Common.Exceptionless;
using Collectively.Common.Nancy;
using Collectively.Common.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Configuration;
using Serilog;
using RawRabbit.Configuration;
using ModuleContainer = Collectively.Api.IoC.ModuleContainer;
using Newtonsoft.Json;
using Collectively.Common.RabbitMq;
using Collectively.Common.Security;
using Collectively.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Collectively.Common.Caching;
using Collectively.Api.Filters;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Api.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly string[] ForbiddenAccountStates = new []{"inactive", "unconfirmed", "locked", "deleted"};
        private static readonly ILogger Logger = Log.Logger;
        private IExceptionHandler _exceptionHandler;
        private static readonly string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string InvalidDecimalSeparator = DecimalSeparator == "." ? "," : ".";
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private bool _validateAccountState = false;

        public Bootstrapper(IConfiguration configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _services = services;
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
            pipelines.SetupTokenAuthentication(container);
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            _validateAccountState = container.Resolve<AppSettings>().ValidateAccountState;
            Logger.Information($"Account state validation is {(_validateAccountState ? "enabled" : "disabled")}.");
            Logger.Information("Collectively API has started.");
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Logger.Information("Configuring IoC");
            base.ConfigureApplicationContainer(container);

            container.Update(builder =>
            {
                builder.Populate(_services);
                builder.RegisterType<CustomJsonSerializer>().As<JsonSerializer>().SingleInstance();
                builder.RegisterType<BrowseRemarksPagedFilter>().As<IPagedFilter<Remark, BrowseRemarks>>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<AppSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<FeatureSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<RedisSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
                builder.RegisterInstance(new MemoryCache(new MemoryCacheOptions())).As<IMemoryCache>().SingleInstance();
                builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
                builder.RegisterModule<ModuleContainer>();
                builder.RegisterModule<RedisModule>();
                builder.RegisterType<AccountStateProvider>().As<IAccountStateProvider>().InstancePerLifetimeScope();
                builder.RegisterType<OperationProvider>().As<IOperationProvider>().InstancePerLifetimeScope();
                SecurityContainer.Register(builder, _configuration);
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
                    "Request details", "Collectively", "API");

                return ctx.Response;
            });
            if(_validateAccountState)
            {
                pipelines.BeforeRequest += async (ctx, token) => {
                    var nancyContext = ctx as NancyContext;
                    if(nancyContext.CurrentUser == null)
                    {
                        return null;
                    }
                    var userId = nancyContext.CurrentUser.Identity.Name;
                    var accountStateProvider = container.Resolve<IAccountStateProvider>();
                    var state = await accountStateProvider.GetAsync(userId);
                    if (state == "unconfirmed" && nancyContext.Request.Method == "GET")
                    {
                        return null;
                    }
                    if (state.Empty() || ForbiddenAccountStates.Contains(state))
                    {
                        return HttpStatusCode.Forbidden;
                    }
                    return null;
                };
            }
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
                .WithHeader("Access-Control-Expose-Headers", "X-Operation,X-Resource,Location,X-Total-Count");
        }
    }
}
