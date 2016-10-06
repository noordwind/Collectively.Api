using Coolector.Api.Modules.Base;
using Coolector.Common.Extensions;
using Coolector.Core.Commands;
using Coolector.Core.Filters;
using Coolector.Core.Storages;

namespace Coolector.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(ICommandDispatcher commandDispatcher, IUserStorage userStorage)
            :base(commandDispatcher, modulePath: "users")
        {
            Get("", async args =>
            {
                var query = BindRequest<BrowseUsers>();
                var users = await userStorage.BrowseAsync(query);

                return FromPagedResult(users.Select(x => new {x.UserId, x.Name}));
            });
        }
    }
}