using Collectively.Api.Commands;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Mailing;

namespace Collectively.Api.Modules
{
    public class SupportModule : ModuleBase
    {
        public SupportModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "support")
        {
            Post("mailing", async args => await For<SendSupportEmailMessage>()
                .Set(x => x.Title = $"Support message from: {x.Email}")
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}