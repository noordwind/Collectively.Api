using Collectively.Api.Tests.EndToEnd.Framework;
using Collectively.Services.Storage.Models.Operations;
using Machine.Specifications;

namespace Collectively.Api.Tests.EndToEnd.Modules
{
    public abstract class ResetPasswordModule_specs : ModuleBase_specs
    {
        protected static Operation InitiateResetPassword()
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
        protected static Operation Result;

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