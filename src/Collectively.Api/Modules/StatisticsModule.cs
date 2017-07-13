using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Services.Storage.Models.Statistics;


namespace Collectively.Api.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(ICommandDispatcher commandDispatcher, 
            IValidatorResolver validatorResolver,
            IStatisticsStorage statisticsStorage) 
            : base(commandDispatcher, validatorResolver, modulePath: "statistics")
        {
            Get("remarks", async args => await FetchCollection<BrowseRemarkStatistics, RemarkStatistics>
                (async x => await statisticsStorage.BrowseRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/{id}", async args => await Fetch<GetRemarkStatistics, RemarkStatistics>
                (async x => await statisticsStorage.GetRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/general", async args => await Fetch<GetRemarksCountStatistics, RemarksCountStatistics>
                (async x => await statisticsStorage.GetRemarksCountStatisticsAsync(x))
                .HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseCategoryStatistics, CategoryStatistics>
                (async x => await statisticsStorage.BrowseCategoryStatisticsAsync(x))
                .HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseTagStatistics, TagStatistics>
                (async x => await statisticsStorage.BrowseTagStatisticsAsync(x))
                .HandleAsync());

            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatistics>
                (async x => await statisticsStorage.BrowseUserStatisticsAsync(x))
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatistics>
                (async x => await statisticsStorage.GetUserStatisticsAsync(x))
                .HandleAsync());
        }
    }
}