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

        public async Task<User> GetAsync(Guid id)
            => await _database.Users().GetByIdAsync(id);

        public async Task<User> GetByEmailAsync(string email)
            => await _database.Users().GetByEmailAsync(email);

        public async Task<User> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);
    }
}