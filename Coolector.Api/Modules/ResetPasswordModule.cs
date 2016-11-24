using Coolector.Api.Commands;
using Coolector.Api.Validation;
using Coolector.Common.Commands.Users;

namespace Coolector.Api.Modules
{
    public class ResetPasswordModule : ModuleBase
    {
        public ResetPasswordModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver) : base(commandDispatcher, validatorResolver, "reset-password")
        {
            Post("", async args => await For<ResetPassword>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Post("set-new", async args => await For<SetNewPassword>()
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}