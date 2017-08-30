using System;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;


namespace Collectively.Api.Storages
{
    public class RemarkStorage : IRemarkStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly Collectively.Common.Caching.ICache _cache;

        public RemarkStorage(IStorageClient storageClient, Collectively.Common.Caching.ICache cache)
        {
            _storageClient = storageClient;
            _cache = cache;
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id)
        {
            var remark = await _cache.GetAsync<Remark>($"remarks:{id}");
            if (remark.HasValue)
            {
                return remark;
            }
            return await _storageClient.GetAsync<Remark>($"remarks/{id}");
        }

        public async Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query)
        {
            //TODO: Implement the proper remarks filtering.
            // var keys = await _cache.GetGeoRadiusAsync("remarks", query.Longitude, query.Latitude, query.Radius);
            // var results = await _cache.GetManyAsync<Remark>(keys
            //     .OrderBy(x => x.Distance)
            //     .Select(x => $"remarks:{x.Name}")
            //     .ToArray());

            // return PagedResult<Remark>.Create(results,1,100,1,100);
            return await _storageClient.GetFilteredCollectionAsync<Remark, BrowseRemarks>(query, "remarks");
        }
        public async Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _storageClient.GetFilteredCollectionAsync<RemarkCategory, BrowseRemarkCategories>
                (query, "remarks/categories");

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await _storageClient.GetFilteredCollectionAsync<Tag, BrowseRemarkTags>
                (query, "remarks/tags");
    }
}