using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Core.Domain.Users;
using Coolector.Infrastructure.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Coolector.Infrastructure.Mongo
{
    public class MongoDatabaseInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly IMongoDatabase _database;

        public MongoDatabaseInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            RegisterConventions();
            var collections = await _database.ListCollectionsAsync();
            var exists = await collections.AnyAsync();
            if (exists)
                return;

            await CreateDatabaseAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("CoolectorConventions", new MongoConvention(), x => true);
            _initialized = true;
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
            };
        }

        private async Task CreateDatabaseAsync()
        {
            await _database.CreateCollectionAsync<User>("Users");
        }
    }
}