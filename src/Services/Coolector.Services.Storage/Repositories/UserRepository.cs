using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Services.Mongo;
using Coolector.Services.Storage.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Storage.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
            => await _database.Users()
                .Query(query)
                .PaginateAsync(query);

        public async Task<Maybe<UserDto>> GetByIdAsync(string id)
            => await _database.Users().GetByIdAsync(id);

        public async Task AddAsync(UserDto user)
            => await _database.Users().InsertOneAsync(user);

        public async Task AddManyAsync(IEnumerable<UserDto> users)
            => await _database.Users().InsertManyAsync(users);
    }
}