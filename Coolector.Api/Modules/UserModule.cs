using Coolector.Api.Queries;
using Coolector.Api.Validation;
using Coolector.Dto.Users;
using BrowseUsers = Coolector.Api.Queries.BrowseUsers;
using ICommandDispatcher = Coolector.Api.Commands.ICommandDispatcher;
using IUserStorage = Coolector.Api.Storages.IUserStorage;

namespace Coolector.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(ICommandDispatcher commandDispatcher,
            IUserStorage userStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, UserDto>
                (async x => await userStorage.BrowseAsync(x)).HandleAsync());

            Get("{name}", async args => await Fetch<GetUserByName, UserDto>
                (async x => await userStorage.GetByNameAsync(x.Name)).HandleAsync());
        }
    }
}