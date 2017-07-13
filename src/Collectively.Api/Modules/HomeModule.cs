using Collectively.Api.Commands;
using Collectively.Api.Validation;

namespace Collectively.Api.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver)
        {
            Get("", args => "Welcome to the Collectively API!");
        }
    }
}