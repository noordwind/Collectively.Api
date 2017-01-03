using Coolector.Api.Commands;
using Coolector.Api.Validation;

namespace Coolector.Api.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver)
        {
            Get("", args => "Welcome to the Coolector API!");
        }
    }
}