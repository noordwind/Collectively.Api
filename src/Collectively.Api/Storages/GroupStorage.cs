using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Caching;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Storages
{
    public class GroupStorage : IGroupStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly ICache _cache;

        public GroupStorage(IStorageClient storageClient,
            ICache cache)
        {
            _storageClient = storageClient;
            _cache = cache;
        }

        public async Task<Maybe<Group>> GetAsync(Guid id)
        {
            var group = await _cache.GetAsync<Group>($"groups:{id}");
            if (group.HasValue)
            {
                return group;
            }

            return await _storageClient.GetAsync<Group>($"groups/{id}");            
        }
        public async Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query)
            => await _storageClient.GetFilteredCollectionAsync<Group, BrowseGroups>(query, "groups");
    }
}