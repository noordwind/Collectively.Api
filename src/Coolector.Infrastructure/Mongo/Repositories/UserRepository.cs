using System;
using System.Threading.Tasks;
using Coolector.Core.Domain.Users;
using Coolector.Core.Repositories;
using Coolector.Infrastructure.Mongo.Queries;
using MongoDB.Driver;

namespace Coolector.Infrastructure.Mongo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public Task<User> GetAsync(Guid id)
            => _database.Users().GetByIdAsync(id);

        public Task<User> GetByEmailAsync(string email)
            => _database.Users().GetByEmailAsync(email);

        public Task<User> GetByNameAsync(string name)
            => _database.Users().GetByNameAsync(name);
    }
}