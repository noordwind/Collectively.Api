using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Groups;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Modules
{
    public class GroupModule : ModuleBase
    {
        public GroupModule(ICommandDispatcher commandDispatcher,
            IGroupStorage groupStorage, 
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "groups")
        {
            Get("{id}", async args => await Fetch<GetGroup, Group>
                (async x => await groupStorage.GetAsync(x.Id)).HandleAsync());

            Get("", async args => await FetchCollection<BrowseGroups, Group>
                (async x => await groupStorage.BrowseAsync(x)).HandleAsync());

            Post("", async args => await For<CreateGroup>()
                .SetResourceId(x => x.GroupId)
                .OnSuccessAccepted("groups/{0}")
                .DispatchAsync());        
        }
    }
}