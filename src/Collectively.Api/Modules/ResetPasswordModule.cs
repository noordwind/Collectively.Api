using Collectively.Api.Commands;
using Collectively.Api.Framework;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Users;

namespace Collectively.Api.Modules
{
    public class ResetPasswordModule : ModuleBase
    {
        public ResetPasswordModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver, AppSettings appSettings)
            : base(commandDispatcher, validatorResolver, "reset-password")
        {
            Post("", async args => await For<ResetPassword>()
                .Set(x => x.Endpoint = appSettings.ResetPasswordUrl)
                .OnSuccessAccepted()
                .DispatchAsync());

            Post("set-new", async args => await For<SetNewPassword>()
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}