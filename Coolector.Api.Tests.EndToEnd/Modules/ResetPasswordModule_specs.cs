using Coolector.Api.Tests.EndToEnd.Framework;
using Coolector.Services.Operations.Shared.Dto;
using Machine.Specifications;

namespace Coolector.Api.Tests.EndToEnd.Modules
{
    public abstract class ResetPasswordModule_specs : ModuleBase_specs
    {
        protected static OperationDto InitiateResetPassword()
        {
            return OperationHandler.PostAsync("reset-password", new
            {
                email = TestEmail
            }).WaitForResult();
        }
    }

    [Subject("Initiate reset password")]
    public class when_initiating_password_reset : ResetPasswordModule_specs
    {
        protected static OperationDto Result;

        Establish context = () =>
        {
            Initialize();
            Wait();
        };

        Because of = () => Result = InitiateResetPassword();

        It should_return_success_status_code = () =>
        {
            Result.Success.ShouldBeTrue();
        };
    }
}