using Autofac;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Users;
using Coolector.Services.Encryption;
using Coolector.Services.Extensions;
using Coolector.Services.Mongo;
using Coolector.Services.Nancy;
using Coolector.Services.Users.Auth0;
using Coolector.Services.Users.Handlers;
using Coolector.Services.Users.Repositories;
using Coolector.Services.Users.Services;
using Microsoft.Extensions.Configuration;
using Nancy.Bootstrapper;
using NLog;
using RawRabbit;
using RawRabbit.vNext;

namespace Coolector.Services.Users.Framework
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
                builder.RegisterInstance(_configuration.GetSettings<Auth0Settings>());
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<Encrypter>().As<IEncrypter>();
                builder.RegisterType<Auth0RestClient>().As<IAuth0RestClient>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterInstance(BusClientFactory.CreateDefault()).As<IBusClient>();
                builder.RegisterType<SignInUserHandler>().As<ICommandHandler<SignInUser>>();
                builder.RegisterType<ChangeUserNameHandler>().As<ICommandHandler<ChangeUserName>>();
            });
            LifetimeScope = container;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            Logger.Info("Coolector.Services.Users API Started");
        }
    }
}