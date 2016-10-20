using Coolector.Api.Commands;

namespace Coolector.Api.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule(ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            Get("", args => "Hello from Nancy running on CoreCLR");
        }
    }
}