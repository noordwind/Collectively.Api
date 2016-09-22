using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Core.Domain.Remarks;
using Coolector.Core.Domain.Users;
using Coolector.Infrastructure.Mongo.Queries;
using Coolector.Infrastructure.Services;
using MongoDB.Driver;

namespace Coolector.Infrastructure.Mongo
{
    public class MongoDatabaseSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;

        public MongoDatabaseSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            await SeedCategoriesAsync();
            await SeedUsersAsync();
        }


        private async Task SeedCategoriesAsync()
        {
            var categories = new List<Category>
            {
                new Category("Litter"),
                new Category("Collected Garbage")
            };

            await _database.Categories().InsertManyAsync(categories);
        }

        private async Task SeedUsersAsync()
        {
            var users = new List<User>
            {
                new User("noordwind-test1@mailinator.com"),
                new User("noordwind-test2@mailinator.com"),
                new User("noordwind=test3@mailinator.com")
            };

            await _database.Users().InsertManyAsync(users);
        }
    }
}