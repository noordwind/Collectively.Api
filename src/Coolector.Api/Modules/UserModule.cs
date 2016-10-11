using Coolector.Core.Commands;
using Coolector.Core.Queries;
using Coolector.Core.Storages;
using Coolector.Dto.Users;

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