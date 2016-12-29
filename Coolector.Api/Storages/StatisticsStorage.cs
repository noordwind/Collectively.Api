using System.Threading.Tasks;
using Coolector.Api.Queries;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;

namespace Coolector.Api.Storages
{
    public class StatisticsStorage : IStatisticsStorage
    {
        private readonly IStorageClient _storageClient;

        private const string UserStatisticsEndpoint = "statistics/users";

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
    }
}