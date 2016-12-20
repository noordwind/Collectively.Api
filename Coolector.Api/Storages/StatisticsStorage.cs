using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;

namespace Coolector.Api.Storages
{
    public class StatisticsStorage : IStatisticsStorage
    {
        private readonly IStorageClient _storageClient;

        public StatisticsStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<PagedResult<ReporterDto>>> BrowseReportersAsync(BrowseReporters query)
            => await _storageClient
                .GetFilteredCollectionAsync<ReporterDto, BrowseReporters>(query, "statistics/reporters");

        public async Task<Maybe<PagedResult<ResolverDto>>> BrowseResolversAsync(BrowseResolvers query)
            => await _storageClient
                .GetFilteredCollectionAsync<ResolverDto, BrowseResolvers>(query, "statistics/resolvers");
    }
}