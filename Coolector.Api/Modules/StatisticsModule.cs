using Coolector.Api.Commands;
using Coolector.Api.Queries;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using Coolector.Services.Statistics.Shared.Dto;

namespace Coolector.Api.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(ICommandDispatcher commandDispatcher, 
            IValidatorResolver validatorResolver,
            IStatisticsStorage statisticsStorage) 
            : base(commandDispatcher, validatorResolver, modulePath: "statistics")
        {
            Get("remarks", async args => await FetchCollection<BrowseRemarkStatistics, RemarkStatisticsDto>
                (async x => await statisticsStorage.BrowseRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/{id}", async args => await Fetch<GetRemarkStatistics, RemarkStatisticsDto>
                (async x => await statisticsStorage.GetRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatisticsDto>
                (async x => await statisticsStorage.BrowseUserStatisticsAsync(x))
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatisticsDto>
                (async x => await statisticsStorage.GetUserStatisticsAsync(x))
                .HandleAsync());
        }
    }
}