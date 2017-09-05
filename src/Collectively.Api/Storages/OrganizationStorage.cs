using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Caching;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Storages
{
    public class OrganizationStorage : IOrganizationStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly ICache _cache;

        public OrganizationStorage(IStorageClient storageClient,
            ICache cache)
        {
            _storageClient = storageClient;
            _cache = cache;
        }

        public async Task<Maybe<Organization>> GetAsync(Guid id)
        {
            var organization = await _cache.GetAsync<Organization>($"organizations:{id}");
            if (organization.HasValue)
            {
                return organization;
            }

            return await _storageClient.GetAsync<Organization>($"organizations/{id}");        
        }

        public async Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query)
            => await _storageClient.GetFilteredCollectionAsync<Organization, BrowseOrganizations>(query, "organizations");
    }
}