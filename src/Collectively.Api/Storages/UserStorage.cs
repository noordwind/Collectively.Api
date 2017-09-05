using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Caching;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Storages
{
    public class UserStorage : IUserStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly ICache _cache;

        public UserStorage(IStorageClient storageClient,
            ICache cache)
        {
            _storageClient = storageClient;
            _cache = cache;
        }

        public async Task<Maybe<AvailableResource>> IsNameAvailableAsync(string name)
            => await _storageClient.GetAsync<AvailableResource>($"users/{name}/available");

        public async Task<Maybe<User>> GetAsync(string id)
        {
            var user = await _cache.GetAsync<User>($"users:{id}");
            if (user.HasValue)
            {
                return user;
            }

            return await _storageClient.GetAsync<User>($"users/{id}");
        }

        public async Task<Maybe<UserInfo>> GetInfoAsync(string id)
        {
            var user = await _cache.GetAsync<UserInfo>($"users:{id}");
            if (user.HasValue)
            {
                return user;
            }

            return await _storageClient.GetAsync<UserInfo>($"users/{id}");
        }

        public async Task<Maybe<User>> GetByNameAsync(string name)
            => await _storageClient.GetAsync<User>($"users/{name}/account");

        public async Task<Maybe<UserInfo>> GetInfoByNameAsync(string name)
            => await _storageClient.GetAsync<UserInfo>($"users/{name}/account");

        public async Task<Maybe<UserSession>> GetSessionAsync(Guid id)
            => await _storageClient.GetAsync<UserSession>($"user-sessions/{id}");

        public async Task<Maybe<PagedResult<UserInfo>>> BrowseAsync(BrowseUsers query)
            => await _storageClient.GetFilteredCollectionAsync<UserInfo, BrowseUsers>(query, "users");
    }
}