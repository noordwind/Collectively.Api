using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Domain;
using Coolector.Services.Users.Domain;
using Coolector.Services.Users.Queries;
using MongoDB.Driver;
using Coolector.Services.Mongo;

namespace Coolector.Services.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<User>> GetByUserIdAsync(string userId)
            => await _database.Users().GetByUserIdAsync(userId);

        public async Task<Maybe<User>> GetByEmailAsync(string email)
            => await _database.Users().GetByEmailAsync(email);

        public async Task<Maybe<User>> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(int page = 1, int results = 10)
        {
            var query = new BrowseUsers
            {
                Page = page,
                Results = results
            };

            return await _database.Users()
                .Query(query)
                .PaginateAsync(query);
        }

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);
    }
}