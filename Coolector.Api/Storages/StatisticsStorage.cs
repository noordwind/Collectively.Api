using System;
using System.Threading.Tasks;
using Coolector.Api.Queries;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;

namespace Coolector.Api.Storages
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

        public async Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<UserStatisticsDto, BrowseUserStatistics>(query, UserStatisticsEndpoint);

        public async Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query)
            => await _storageClient
                .GetAsync<UserStatisticsDto>($"{UserStatisticsEndpoint}/{query.Id}");

        public async Task<Maybe<PagedResult<RemarkStatisticsDto>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<RemarkStatisticsDto, BrowseRemarkStatistics>(query, RemarkStatisticsEndpoint);

        public async Task<Maybe<RemarkStatisticsDto>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
            => await _storageClient
                .GetAsync<RemarkStatisticsDto>($"{RemarkStatisticsEndpoint}/{query.Id}");

        public async Task<Maybe<RemarkGeneralStatisticsDto>> GetRemarkGeneralStatisticsAsync(GetRemarkGeneralStatistics query)
            => await _storageClient
                .GetAsync<RemarkGeneralStatisticsDto>($"{RemarkStatisticsEndpoint}/general".ToQueryString(query));

        public async Task<Maybe<PagedResult<CategoryStatisticsDto>>> BrowseCategoryStatisticsAsync(
                BrowseCategoryStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<CategoryStatisticsDto, BrowseCategoryStatistics>(query, CategoryStatisticsEndpoint);

        public async Task<Maybe<PagedResult<TagStatisticsDto>>> BrowseTagStatisticsAsync(BrowseTagStatistics query)
            => await _storageClient
                .GetFilteredCollectionAsync<TagStatisticsDto, BrowseTagStatistics>(query, TagStatisticsEndpoint);
    }
}