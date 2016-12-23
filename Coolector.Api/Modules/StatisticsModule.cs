using Coolector.Api.Commands;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;

namespace Coolector.Api.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(ICommandDispatcher commandDispatcher, 
            IValidatorResolver validatorResolver,
            IStatisticsStorage statisticsStorage) 
            : base(commandDispatcher, validatorResolver, modulePath: "statistics")
        {
            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatisticsDto>
                (async x => await statisticsStorage.BrowseUserStatisticsAsync(x))
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatisticsDto>
                (async x => await statisticsStorage.GetUserStatisticsAsync(x))
                .HandleAsync());
        }
    }
}