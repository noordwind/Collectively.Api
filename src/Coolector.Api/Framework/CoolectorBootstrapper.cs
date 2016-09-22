using System.Reflection;
using Autofac;
using Coolector.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Coolector.Infrastructure.Settings;
using NLog;

namespace Coolector.Api.Framework
{
    public class CoolectorBootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;

        public CoolectorBootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<DatabaseSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();

            if (databaseSettings.Seed)
            {
                var seeder = container.Resolve<IDatabaseSeeder>();
                seeder.SeedAsync();
            }

            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };

            Logger.Info("API Started");
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Logger.Info("Configuring IoC");
            base.ConfigureApplicationContainer(container);

            container.Update(builder =>
            {
                builder.Register(x => GetConfigurationValue<GeneralSettings>("general"))
                    .As<GeneralSettings>();
                builder.Register(x => GetConfigurationValue<DatabaseSettings>("database"))
                    .As<DatabaseSettings>();
                builder.Register(x => GetConfigurationValue<Auth0Settings>("auth0"))
                    .As<Auth0Settings>();

                builder.RegisterModule<Infrastructure.IoC.ModuleContainer>();
            });
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
        }

        private T GetConfigurationValue<T>(string section) where T : new()
        {
            var configurationValue = new T();
            _configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }
    }
}
