using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Core.Filters;

namespace Coolector.Core.Storages
{
    public class UserStorage : IUserStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly IFilter<UserDto, BrowseUsers> _usersFilter;

        public UserStorage(IStorageClient storageClient, IFilter<UserDto, BrowseUsers> usersFilter)
        {
            _storageClient = storageClient;
            _usersFilter = usersFilter;
        }

        public async Task<Maybe<UserDto>> GetAsync(string id)
            => await _storageClient.GetUsingCacheAsync<UserDto>($"users/{id}");

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
        {
            var users = await _storageClient.GetCollectionUsingCacheAsync<UserDto>("users");

            return _usersFilter.Filter(users, query);
        }
    }
}