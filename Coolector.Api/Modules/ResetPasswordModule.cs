using Coolector.Api.Commands;
using Coolector.Api.Framework;
using Coolector.Api.Validation;
using Coolector.Services.Users.Shared.Commands;

namespace Coolector.Api.Modules
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