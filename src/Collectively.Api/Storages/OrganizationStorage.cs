using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Storages
{
    public class OrganizationStorage : IOrganizationStorage
    {
        private readonly IStorageClient _storageClient;

        public OrganizationStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<Organization>> GetAsync(Guid id)
            => await _storageClient.GetAsync<Organization>($"organizations/{id}");

        public async Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query)
            => await _storageClient.GetFilteredCollectionAsync<Organization, BrowseOrganizations>(query, "organizations");
    }
}