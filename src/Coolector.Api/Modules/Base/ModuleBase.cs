using Coolector.Infrastructure.Commands;
using Nancy;

namespace Coolector.Api.Modules.Base
{
    public class ModuleBase : NancyModule
    {
        protected readonly ICommandDispatcher CommandDispatcher;

        public ModuleBase(ICommandDispatcher commandDispatcher, string modulePath = "")
            :base(modulePath)
        {
            CommandDispatcher = commandDispatcher;
        }
    }
}