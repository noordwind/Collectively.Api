using Coolector.Api.Commands;
using Coolector.Api.Validation;
using Coolector.Common.Commands.Facebook;

namespace Coolector.Api.Modules
{
    public class FacebookModule : ModuleBase
    {
        public FacebookModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, "social/facebook")
        {
            Post("wall", async args => await For<PostMessageOnFacebookWall>()
                .OnSuccessAccepted("account")
                .DispatchAsync());
        }
    }
}