using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Groups;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Modules
{
    public class OrganizationModule : ModuleBase
    {
        public OrganizationModule(ICommandDispatcher commandDispatcher,
            IOrganizationStorage organizationStorage, 
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "organizations")
        {
            Get("{id}", async args => await Fetch<GetOrganization, Organization>
                (async x => await organizationStorage.GetAsync(x.Id)).HandleAsync());

            Get("", async args => await FetchCollection<BrowseOrganizations, Organization>
                (async x => await organizationStorage.BrowseAsync(x))
                .MapTo(x => new BasicOrganization
                {
                    Id = x.Id,
                    Name = x.Name,
                    Codename = x.Codename,
                    IsPublic = x.IsPublic,
                    State = x.State,
                    CreatedAt = x.CreatedAt,
                    MembersCount = x.Members?.Count ?? 0,
                    GroupsCount = x.Groups?.Count ?? 0
                }).HandleAsync());
                
            Post("", async args => await For<CreateOrganization>()
                .SetResourceId(x => x.OrganizationId)
                .OnSuccessAccepted("organizations/{0}")
                .DispatchAsync());   

            Post("{organizationId}/members", async args => await ForModerator<AddMemberToOrganization>()
                .OnSuccessAccepted("organizations/{0}")
                .DispatchAsync());  
        }
    }
}