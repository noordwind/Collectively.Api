using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Remarks;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Api.Modules
{
    public class TagsModule :  ModuleBase
    {
        public TagsModule(ICommandDispatcher commandDispatcher, 
            IValidatorResolver validatorResolver,
            IRemarkStorage remarkStorage) 
            : base(commandDispatcher, validatorResolver, modulePath: "tags")
        {

            Get("", async args => await FetchCollection<BrowseRemarkTags, Tag>
                (async x => await remarkStorage.BrowseTagsAsync(x)).HandleAsync());

            Post("", async args => await ForAdministrator<CreateTags>()
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}