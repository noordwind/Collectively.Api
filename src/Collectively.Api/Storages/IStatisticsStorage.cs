using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Statistics;

namespace Collectively.Api.Storages
{
    public interface IStatisticsStorage
    {
        Task<Maybe<PagedResult<UserStatistics>>> BrowseUserStatisticsAsync(BrowseUserStatistics query);
        Task<Maybe<UserStatistics>> GetUserStatisticsAsync(GetUserStatistics query);
        Task<Maybe<PagedResult<RemarkStatistics>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query);
        Task<Maybe<RemarkStatistics>> GetRemarkStatisticsAsync(GetRemarkStatistics query);
        Task<Maybe<RemarksCountStatistics>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query);
        Task<Maybe<PagedResult<CategoryStatistics>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query);
        Task<Maybe<PagedResult<TagStatistics>>> BrowseTagStatisticsAsync(BrowseTagStatistics query);
    }
}