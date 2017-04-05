using Collectively.Api.Commands;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Remarks;

namespace Collectively.Api.Modules
{
    public class RemarkActionModule : ModuleBase
    {
        public RemarkActionModule(ICommandDispatcher commandDispatcher,
            IRemarkStorage remarkStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "remarks/{remarkId}/actions")
        {
            Post("", async args => await For<TakeRemarkAction>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Delete("", async args => await For<CancelRemarkAction>()
                .OnSuccessAccepted()
                .DispatchAsync());              
        }
    }
}