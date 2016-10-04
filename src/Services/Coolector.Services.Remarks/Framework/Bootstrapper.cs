using Autofac;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Services.Extensions;
using Coolector.Services.Mongo;
using Coolector.Services.Nancy;
using Coolector.Services.Remarks.Handlers;
using Coolector.Services.Remarks.Repositories;
using Coolector.Services.Remarks.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Nancy.Bootstrapper;
using NLog;
using RawRabbit;
using RawRabbit.vNext;

namespace Coolector.Services.Remarks.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);
            container.Update(builder =>
            {
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>());
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
                builder.RegisterType<DatabaseSeeder>().As<IDatabaseSeeder>();
                builder.RegisterType<RemarkRepository>().As<IRemarkRepository>();
                builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<RemarkService>().As<IRemarkService>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterType<FileHandler>().As<IFileHandler>();
                builder.RegisterType<FileResolver>().As<IFileResolver>();
                builder.RegisterInstance(BusClientFactory.CreateDefault()).As<IBusClient>();
                builder.RegisterType<CreaterRemarkHandler>().As<ICommandHandler<CreateRemark>>();
                builder.RegisterType<NewUserSignedInHandler>().As<IEventHandler<NewUserSignedIn>>();
            });
            LifetimeScope = container;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            if (databaseSettings.Seed)
            {
                var databaseSeeder = container.Resolve<IDatabaseSeeder>();
                databaseSeeder.SeedAsync();
            }
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            Logger.Info("Coolector.Services.Remarks API Started");
        }
    }
}