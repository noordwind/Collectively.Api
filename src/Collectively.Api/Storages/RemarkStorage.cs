using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Api.Framework;
using Collectively.Api.Queries;
using Collectively.Common.Caching;
using Collectively.Common.Locations;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Api.Storages
{
    public class RemarkStorage : IRemarkStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly IPagedFilter<Remark, BrowseRemarks> _browseRemarksFilter;
        private readonly IPagedFilter<Remark, BrowseSimilarRemarks> _browseSimilarRemarksFilter;
        private readonly Collectively.Common.Caching.ICache _cache;
        private readonly bool _useCache;

        public RemarkStorage(IStorageClient storageClient, 
            IPagedFilter<Remark, BrowseRemarks> browseRemarksFilter,
            IPagedFilter<Remark, BrowseSimilarRemarks> browseSimilarRemarksFilter,
            Collectively.Common.Caching.ICache cache,
            RedisSettings redisSettings)
        {
            _storageClient = storageClient;
            _cache = cache;
            _browseRemarksFilter = browseRemarksFilter;
            _browseSimilarRemarksFilter = browseSimilarRemarksFilter;
            _useCache = redisSettings.Enabled;
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
            if (!_useCache)
            {
                return await _storageClient.GetFilteredCollectionAsync<Remark, BrowseRemarks>(query, "remarks");
            }
            if (!query.IsLocationProvided())
            {
                return _browseRemarksFilter.Filter(await GetRemarksWithoutLocationAsync(query), query);
            }

            return _browseRemarksFilter.Filter(await GetRemarksByLocationAsync(query), query);
        }

        public async Task<Maybe<PagedResult<Remark>>> BrowseSimilarAsync(BrowseSimilarRemarks query)
        => _useCache ? 
            _browseSimilarRemarksFilter.Filter(await GetRemarksByLocationAsync(query), query) : 
            await _storageClient.GetFilteredCollectionAsync<Remark, BrowseSimilarRemarks>(query, "remarks/similar");

        private async Task<IEnumerable<Remark>> GetRemarksWithoutLocationAsync(BrowseRemarksBase query)
        {
            var latestKeys = await _cache.GetSortedSetAsync("remarks-latest");
            var remarks = await _cache.GetManyAsync<Remark>(latestKeys
                    .Select(x => $"remarks:{x}")
                    .ToArray());
            remarks = remarks.Where(x => x != null);

            return remarks;
        }

        private async Task<IEnumerable<Remark>> GetRemarksByLocationAsync(BrowseRemarksBase query)
        {
            var radius = query.Radius > 0 ? query.Radius : 10000;
            var geoKeys = await _cache.GetGeoRadiusAsync("remarks", 
                query.Longitude, query.Latitude, radius);
            var remarks = await _cache.GetManyAsync<Remark>(geoKeys
                .OrderBy(x => x.Distance)
                .Select(x => $"remarks:{x.Name}")
                .ToArray());
            remarks = remarks.Where(x => x != null);

            return remarks;
        }

        public async Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _storageClient.GetFilteredCollectionAsync<RemarkCategory, BrowseRemarkCategories>
                (query, "remarks/categories");

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await _storageClient.GetFilteredCollectionAsync<Tag, BrowseRemarkTags>
                (query, "remarks/tags");
    }
}