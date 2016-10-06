using Coolector.Api.Modules.Base;
using Coolector.Common.Commands.Users;
using Coolector.Core.Commands;
using Coolector.Core.Storages;
using Nancy.Security;

namespace Coolector.Api.Modules
{
    public class AccountModule : ModuleBase
    {
        public AccountModule(ICommandDispatcher commandDispatcher, IUserStorage userStorage)
            : base(commandDispatcher)
        {
            this.RequiresAuthentication();

            Get("account", async args =>
            {
                var user = await userStorage.GetAsync(CurrentUserId);
                return user.Value;
            });

            Post("sign-in", async args =>
            {
                var command = BindRequest<SignInUser>();
                await CommandDispatcher.DispatchAsync(command);
            });

            Put("account/username", async args =>
            {
                var command = BindAuthenticatedCommand<ChangeUserName>();
                await CommandDispatcher.DispatchAsync(command);
            });

            Put("account/avatar", async args =>
            {
                var command = BindAuthenticatedCommand<ChangeAvatar>();
                await CommandDispatcher.DispatchAsync(command);
            });
        }
    }
}