using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Storages
{
    public class GroupStorage : IGroupStorage
    {
        private readonly IStorageClient _storageClient;

        public GroupStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<Group>> GetAsync(Guid id)
            => await _storageClient.GetAsync<Group>($"groups/{id}");

        public async Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query)
            => await _storageClient.GetFilteredCollectionAsync<Group, BrowseGroups>(query, "groups");
    }
}