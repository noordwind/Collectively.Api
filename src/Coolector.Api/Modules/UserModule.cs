using Coolector.Api.Modules.Base;
using Coolector.Core.Commands;
using Coolector.Core.Commands.Users;
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
        }
    }
}