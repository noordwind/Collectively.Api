using Autofac.Core;
using Coolector.Api.Modules.Base;
using Coolector.Common.Commands.Users;
using Coolector.Core.Commands;
using Nancy.ModelBinding;

namespace Coolector.Api.Modules
{
    public class UserModule : AuthenticatedModule
    {
        public UserModule(ICommandDispatcher commandDispatcher)
            :base(commandDispatcher)
        {
            Post("sign-in", async args =>
            {
                var command = this.Bind<SignInUser>();
                await CommandDispatcher.DispatchAsync(command);
            });

            Put("users/edit", async args =>
            {
                var command = BindAuthenticatedCommand<EditUser>();
                await CommandDispatcher.DispatchAsync(command);
            });
        }
    }
}