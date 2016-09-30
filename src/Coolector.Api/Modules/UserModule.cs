using System.Collections.Generic;
using Coolector.Api.Modules.Base;
using Coolector.Core.Commands;
using Coolector.Core.Filters;
using Coolector.Core.Storages;
using Nancy.ModelBinding;

namespace Coolector.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(ICommandDispatcher commandDispatcher, IUserStorage userStorage)
            :base(commandDispatcher, modulePath: "users")
        {
            Get("", async args =>
            {
                var query = this.Bind<BrowseUsers>();
                var users = await userStorage.BrowseAsync(query);

                return FromPagedResult(users);
            });
        }
    }
}