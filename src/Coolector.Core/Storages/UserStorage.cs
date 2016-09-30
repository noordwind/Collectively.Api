using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Core.Filters;

namespace Coolector.Core.Storages
{
    public class UserStorage : IUserStorage
    {
        private readonly IStorageClient _storageClient;

        public UserStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<UserDto>> GetAsync(string id)
            => await _storageClient.GetUsingCacheAsync<UserDto>($"users/{id}");

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
            => await _storageClient.GetFilteredCollectionUsingCacheAsync<UserDto, BrowseUsers>(query, "users");
    }
}