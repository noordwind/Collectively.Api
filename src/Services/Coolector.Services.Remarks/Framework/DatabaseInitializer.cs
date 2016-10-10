using System.Threading.Tasks;
using Coolector.Services.Mongo;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Remarks.Framework
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;

        public DatabaseSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            await _database.Categories().InsertOneAsync(new Category("litter"));
            await _database.Categories().InsertOneAsync(new Category("damages"));
            await _database.Categories().InsertOneAsync(new Category("accidents"));
        }
    }
}