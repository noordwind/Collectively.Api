using Coolector.Api.Modules.Base;
using Coolector.Common.Commands.Users;
using Coolector.Core.Commands;

namespace Coolector.Api.Modules
{
    public class AccountModule : AuthenticatedModule
    {
        public AccountModule(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
            Post("sign-in", async args =>
            {
                var command = this.Bind<SignInUser>();
                await CommandDispatcher.DispatchAsync(command);
            });

            Put("me/edit", async args =>
            {
                var command = BindAuthenticatedCommand<EditUser>();
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