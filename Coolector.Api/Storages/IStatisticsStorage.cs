using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;

namespace Coolector.Api.Storages
{
    public interface IStatisticsStorage
    {
        Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query);
        Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query);
    }
}