using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Extensions;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Statistics;


namespace Collectively.Api.Storages
{
    public class StatisticsStorage : IStatisticsStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly string RemarkStatisticsEndpoint = "statistics/remarks";
        private readonly string UserStatisticsEndpoint = "statistics/users";
        private readonly string CategoryStatisticsEndpoint = "statistics/categories";
        private readonly string TagStatisticsEndpoint = "statistics/tags";

        public StatisticsStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<PagedResult<UserStatistics>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<UserStatistics, BrowseUserStatistics>(query, UserStatisticsEndpoint);

        public async Task<Maybe<UserStatistics>> GetUserStatisticsAsync(GetUserStatistics query)
            => await _storageClient
                .GetAsync<UserStatistics>($"{UserStatisticsEndpoint}/{query.Id}");

        public async Task<Maybe<PagedResult<RemarkStatistics>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<RemarkStatistics, BrowseRemarkStatistics>(query, RemarkStatisticsEndpoint);

        public async Task<Maybe<RemarkStatistics>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
            => await _storageClient
                .GetAsync<RemarkStatistics>($"{RemarkStatisticsEndpoint}/{query.Id}");

        public async Task<Maybe<RemarksCountStatistics>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query)
            => await _storageClient
                .GetAsync<RemarksCountStatistics>($"{RemarkStatisticsEndpoint}/general".ToQueryString(query));

        public async Task<Maybe<PagedResult<CategoryStatistics>>> BrowseCategoryStatisticsAsync(
                BrowseCategoryStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<CategoryStatistics, BrowseCategoryStatistics>(query, CategoryStatisticsEndpoint);

        public async Task<Maybe<PagedResult<TagStatistics>>> BrowseTagStatisticsAsync(BrowseTagStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<TagStatistics, BrowseTagStatistics>(query, TagStatisticsEndpoint);
    }
}