using Autofac;
using MongoDB.Driver;

namespace Coolector.Services.Mongo
{
    public class MongoDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                var settings = c.Resolve<MongoDbSettings>();

                return new MongoClient(settings.ConnectionString);
            }).SingleInstance();

            builder.Register((c, p) =>
            {
                var mongoClient = c.Resolve<MongoClient>();
                var settings = c.Resolve<MongoDbSettings>();
                var database = mongoClient.GetDatabase(settings.Database);

                return database;
            }).As<IMongoDatabase>();
        }
    }
}