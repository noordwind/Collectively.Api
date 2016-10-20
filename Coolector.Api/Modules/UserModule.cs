using Coolector.Dto.Users;
using BrowseUsers = Coolector.Api.Queries.BrowseUsers;
using ICommandDispatcher = Coolector.Api.Commands.ICommandDispatcher;
using IUserStorage = Coolector.Api.Storages.IUserStorage;

namespace Coolector.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(ICommandDispatcher commandDispatcher, IUserStorage userStorage)
            : base(commandDispatcher, modulePath: "users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, UserDto>
                (async x => await userStorage.BrowseAsync(x)).HandleAsync());
        }
    }
}