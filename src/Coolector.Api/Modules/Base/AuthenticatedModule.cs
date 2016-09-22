using Coolector.Infrastructure.Commands;
using Nancy.Security;

namespace Coolector.Api.Modules.Base
{
    public class AuthenticatedModule : ModuleBase
    {
        public AuthenticatedModule(ICommandDispatcher commandDispatcher, string modulePath = "")
            : base(commandDispatcher, modulePath)
        {
            this.RequiresAuthentication();
        }
    }
}