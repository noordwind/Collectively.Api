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
            Get("reporters", async args => await FetchCollection<BrowseReporters, ReporterDto>
                (async x => await statisticsStorage.BrowseReportersAsync(x))
                .HandleAsync());

            Get("resolvers", async args => await FetchCollection<BrowseResolvers, ResolverDto>
                (async x => await statisticsStorage.BrowseResolversAsync(x))
                .HandleAsync());
        }
    }
}