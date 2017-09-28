using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Api.Framework;
using Collectively.Api.Queries;
using Collectively.Common.Caching;
using Collectively.Common.Extensions;
using Collectively.Common.Locations;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Api.Storages
{
    public class RemarkStorage : IRemarkStorage
    {
        private static readonly IList<string> RemarkMemberCriteria = new []{"member", "moderator", "administrator", "owner"};
        private readonly IStorageClient _storageClient;
        private readonly IPagedFilter<Remark, BrowseRemarks> _browseRemarksFilter;
        private readonly IPagedFilter<Remark, BrowseSimilarRemarks> _browseSimilarRemarksFilter;
        private readonly ICache _cache;
        private readonly bool _useCache;

        public RemarkStorage(IStorageClient storageClient, 
            IPagedFilter<Remark, BrowseRemarks> browseRemarksFilter,
            IPagedFilter<Remark, BrowseSimilarRemarks> browseSimilarRemarksFilter,
            ICache cache,
            RedisSettings redisSettings)
        {
            _storageClient = storageClient;
            _cache = cache;
            _browseRemarksFilter = browseRemarksFilter;
            _browseSimilarRemarksFilter = browseSimilarRemarksFilter;
            _useCache = redisSettings.Enabled;
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id, string userId)
        {
            var remark = await _cache.GetAsync<Remark>($"remarks:{id}");
            if (remark.HasNoValue)
            {
                remark = await _storageClient.GetAsync<Remark>($"remarks/{id}");
            }
            if (remark.HasNoValue)
            {
                return null;
            }
            if (userId.Empty())
            {
                return remark;
            }
            if (remark.Value.Group == null)
            {
                return remark;
            }
            var groupMemberCriteria = await GetGroupMemberCriteriaAsync(remark.Value.Group.Id, userId);
            remark.Value.Group.MemberRole = groupMemberCriteria.role;
            remark.Value.Group.MemberCriteria = groupMemberCriteria.criteria;
            
            return remark;
        }

        public async Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query)
        {
            if (!_useCache)
            {
                return await _storageClient.GetFilteredCollectionAsync<Remark, BrowseRemarks>(query, "remarks");
            }
            if (!query.IsLocationProvided() || query.SkipLocation)
            {
                return _browseRemarksFilter.Filter(await GetRemarksWithoutLocationAsync(query), query);
            }

            return _browseRemarksFilter.Filter(await GetRemarksByLocationAsync(query), query);
        }

        public async Task<Maybe<PagedResult<Remark>>> BrowseSimilarAsync(BrowseSimilarRemarks query)
        => _useCache ? 
            _browseSimilarRemarksFilter.Filter(await GetRemarksByLocationAsync(query), query) : 
            await _storageClient.GetFilteredCollectionAsync<Remark, BrowseSimilarRemarks>(query, "remarks/similar");

        private async Task<IEnumerable<Remark>> GetRemarksWithoutLocationAsync(BrowseRemarks query)
        {
            var keys = Enumerable.Empty<string>();
            if (query.AuthorId.NotEmpty())
            {
                keys = await _cache.GetSetAsync($"users:{query.AuthorId}:remarks");
            }
            else if (query.ResolverId.NotEmpty())
            {
                keys = await _cache.GetSetAsync($"users:{query.ResolverId}:remarks");
            }
            else
            {
                keys = await _cache.GetSortedSetAsync("remarks-latest");
            }
            if (keys == null || !keys.Any()) 
            {
                return Enumerable.Empty<Remark>();
            }
            var remarks = await _cache.GetManyAsync<Remark>(keys
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
        {
            var categories = await _cache.GetSetAsync<RemarkCategory>("categories");
            if (categories?.Any() == true)
            {
                return categories.Paginate(1, int.MaxValue);
            }

            return await _storageClient.GetFilteredCollectionAsync<RemarkCategory, BrowseRemarkCategories>
                (query, "remarks/categories");
        }

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
        {
            var tags = await _cache.GetSetAsync<Tag>("tags");
            if (tags?.Any() == true)
            {
                return tags.Paginate(1, int.MaxValue);
            }

            return await _storageClient.GetFilteredCollectionAsync<Tag, BrowseRemarkTags>
                (query, "remarks/tags");            
        }

        private async Task<(string role, IList<string> criteria)> GetGroupMemberCriteriaAsync(Guid id, string userId)
        {
            var criteria = new List<string>();
            var group = await _cache.GetAsync<Group>($"groups:{id}");
            if (group.HasNoValue || group.Value.Criteria == null) 
            {
                return (string.Empty, criteria);
            }
            var member = group.Value.Members.SingleOrDefault(x => x.UserId == userId);
            if (member == null || !member.IsActive)
            {
                return (string.Empty, criteria);
            }
            var memberRoleIndex = RemarkMemberCriteria.IndexOf(member.Role);
            foreach (var criterion in group.Value.Criteria)
            {
                var requiredRole = criterion.Value.FirstOrDefault();
                if (requiredRole.Empty())
                {
                    continue;
                }
                if (requiredRole == "public")
                {
                    criteria.Add(criterion.Key);
                    continue;
                }
                if (!RemarkMemberCriteria.Contains(requiredRole))
                {
                    continue;
                }
                var requiredRoleIndex = RemarkMemberCriteria.IndexOf(requiredRole);
                if (memberRoleIndex >= requiredRoleIndex)
                {
                    criteria.Add(criterion.Key);
                }
            }

            return (member.Role, criteria);
        }
    }
}