using Coolector.Api.Modules.Base;
using Coolector.Common.Commands.Users;
using Coolector.Core.Commands;
using Nancy.ModelBinding;

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

            Put("me/nickname", async args =>
            {
                var command = BindAuthenticatedCommand<ChangeUserName>();
                await CommandDispatcher.DispatchAsync(command);
            });
        }
    }
}