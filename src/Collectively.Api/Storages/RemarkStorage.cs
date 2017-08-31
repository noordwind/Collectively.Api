using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IPagedFilter<Remark, BrowseRemarks> _filter;
        private readonly Collectively.Common.Caching.ICache _cache;
        private readonly bool _useCache;

        public RemarkStorage(IStorageClient storageClient, 
            IPagedFilter<Remark, BrowseRemarks> filter,
            Collectively.Common.Caching.ICache cache,
            RedisSettings redisSettings)
        {
            _storageClient = storageClient;
            _cache = cache;
            _filter = filter;
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
            if (!query.IsLocationProvided)
            {
                var latestKeys = await _cache.GetSortedSetAsync("remarks-latest");
                var remarks  = await _cache.GetManyAsync<Remark>(latestKeys
                        .Select(x => $"remarks:{x}")
                        .ToArray());

                return _filter.Filter(remarks, query);
            }
            var geoKeys = await _cache.GetGeoRadiusAsync("remarks", 
                query.Longitude, query.Latitude, query.Radius);
            var results = await _cache.GetManyAsync<Remark>(geoKeys
                .OrderBy(x => x.Distance)
                .Select(x => $"remarks:{x.Name}")
                .ToArray());
            var center = new Coordinates(query.Latitude, query.Longitude);
            foreach (var remark in results)
            {
                var coordinates = new Coordinates(remark.Location.Latitude, remark.Location.Longitude);
                remark.Distance = center.DistanceTo(coordinates, UnitOfLength.Meters);
            }

            return _filter.Filter(results, query);
        }
        public async Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _storageClient.GetFilteredCollectionAsync<RemarkCategory, BrowseRemarkCategories>
                (query, "remarks/categories");

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await _storageClient.GetFilteredCollectionAsync<Tag, BrowseRemarkTags>
                (query, "remarks/tags");
    }
}