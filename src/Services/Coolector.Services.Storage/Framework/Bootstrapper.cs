using System.Collections.Generic;
using System.Linq;
using Autofac;
using Coolector.Services.Extensions;
using Coolector.Services.Mongo;
using Coolector.Services.Nancy;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Framework.IoC;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using NLog;
using RawRabbit;
using RawRabbit.vNext;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Coolector.Services.Storage.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;

        public static ILifetimeScope LifeTimeScope { get; private set; }

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
            Logger.Info("Coolector.Services.Storage Configuring application container");
            base.ConfigureApplicationContainer(container);
            container.Update(builder =>
            {
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>());
                builder.RegisterInstance(_configuration.GetSettings<ProviderSettings>());
                builder.RegisterModule<MongoDbModule>();
                builder.Register(c =>
                    {
                        var database = c.Resolve<IMongoDatabase>();
                        var bucket = new GridFSBucket(database);

                        return bucket;
                    })
                    .As<IGridFSBucket>()
                    .SingleInstance();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterInstance(BusClientFactory.CreateDefault()).As<IBusClient>();
                builder.RegisterType<FileHandler>().As<IFileHandler>();
                builder.RegisterType<RemarkRepository>().As<IRemarkRepository>();
                builder.RegisterType<RemarkCategoryRepository>().As<IRemarkCategoryRepository>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<CustomHttpClient>().As<IHttpClient>();
                builder.RegisterType<ServiceClient>().As<IServiceClient>();
                builder.RegisterType<ProviderClient>().As<IProviderClient>();
                builder.RegisterType<RemarkProvider>().As<IRemarkProvider>();
                builder.RegisterType<UserProvider>().As<IUserProvider>();
                builder.RegisterModule<MapperModule>();
                builder.RegisterModule<EventHandlersModule>();
            });
            LifeTimeScope = container;
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
                ctx.Response.Headers.Add("Access-Control-Allow-Headers",
                    "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            Logger.Info("Coolector.Services.Storage API Started");
        }

        private void FixNumberFormat(NancyContext ctx)
        {
            if (ctx.Request.Query == null)
                return;

            var fixedNumbers = new Dictionary<string, double>();
            foreach (var key in ctx.Request.Query)
            {
                var value = ctx.Request.Query[key].ToString();
                if (!value.Contains("."))
                    continue;

                var number = 0;
                if (int.TryParse(value.Split('.')[0], out number))
                    fixedNumbers[key] = double.Parse(value.Replace(".", ","));
            }
            foreach (var fixedNumber in fixedNumbers)
            {
                ctx.Request.Query[fixedNumber.Key] = fixedNumber.Value;
            }
        }
    }
}