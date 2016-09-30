using Autofac;
using Coolector.Core.IoC;
using Coolector.Core.Storages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using NLog;
using RawRabbit;
using RawRabbit.vNext;

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
                builder.RegisterInstance(GetConfigurationValue<StorageSettings>()).SingleInstance();
                builder.RegisterInstance(BusClientFactory.CreateDefault()).As<IBusClient>();
                builder.RegisterInstance(new MemoryCache(new MemoryCacheOptions())).As<IMemoryCache>().SingleInstance();
                builder.RegisterModule<ModuleContainer>();
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

        private T GetConfigurationValue<T>(string section = "") where T : new()
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                section = typeof(T).Name.Replace("Settings", string.Empty);
            }

            var configurationValue = new T();
            _configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }
    }
}
