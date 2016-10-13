using System.Collections.Generic;
using System.Globalization;
using Autofac;
using Coolector.Common.Commands;
using Coolector.Services.Encryption;
using Coolector.Services.Extensions;
using Coolector.Services.Mongo;
using Coolector.Services.Nancy;
using Coolector.Services.Users.Auth0;
using Coolector.Services.Users.Repositories;
using Coolector.Services.Users.Services;
using Microsoft.Extensions.Configuration;
using Nancy.Bootstrapper;
using NLog;
using RawRabbit;
using RawRabbit.vNext;
using System.Reflection;
using Nancy;
using Nancy.Configuration;

namespace Coolector.Services.Users.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string InvalidDecimalSeparator = DecimalSeparator == "." ? "," : ".";
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

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

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);
            container.Update(builder =>
            {
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>());
                builder.RegisterInstance(_configuration.GetSettings<Auth0Settings>());
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<Encrypter>().As<IEncrypter>();
                builder.RegisterType<Auth0RestClient>().As<IAuth0RestClient>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterInstance(BusClientFactory.CreateDefault()).As<IBusClient>();

                var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
                builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            });
            LifetimeScope = container;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();

            pipelines.BeforeRequest += (ctx) =>
            {
                FixNumberFormat(ctx);

                return null;
            };
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            Logger.Info("Coolector.Services.Users API Started");
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
    }
}