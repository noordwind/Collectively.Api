using Autofac;
using Coolector.Infrastructure.Mongo;
using Coolector.Infrastructure.Services;
using Coolector.Infrastructure.Settings;
using MongoDB.Driver;

namespace Coolector.Infrastructure.IoC.Modules
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                var settings = c.Resolve<DatabaseSettings>();

                return new MongoClient(settings.ConnectionString);
            }).SingleInstance();

            builder.Register((c, p) =>
            {
                var mongoClient = c.Resolve<MongoClient>();
                var settings = c.Resolve<DatabaseSettings>();
                var database = mongoClient.GetDatabase(settings.Database);

                return database;
            }).As<IMongoDatabase>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MongoDatabaseSeeder>()
                .As<IDatabaseSeeder>()
                .SingleInstance();

            builder.RegisterType<MongoDatabaseInitializer>()
                .As<IDatabaseInitializer>()
                .SingleInstance();

            builder.RegisterModule<MongoRepositoryModule>();
        }
    }
}