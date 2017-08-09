using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Storages
{
    public class UserStorage : IUserStorage
    {
        private readonly IStorageClient _storageClient;

        public UserStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<AvailableResource>> IsNameAvailableAsync(string name)
            => await _storageClient.GetAsync<AvailableResource>($"users/{name}/available");

        public async Task<Maybe<User>> GetAsync(string id)
            => await _storageClient.GetAsync<User>($"users/{id}");

        public async Task<Maybe<UserInfo>> GetInfoAsync(string id)
            => await _storageClient.GetAsync<UserInfo>($"users/{id}");

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