using Coolector.Api.Authentication;
using Coolector.Api.Commands;
using Coolector.Api.Validation;
using Coolector.Common.Commands.Users;
using Nancy;

namespace Coolector.Api.Modules
{
    public class AuthenticationModule : ModuleBase
    {
        public AuthenticationModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IUserSessionProvider userSessionProvider,
            IJwtTokenHandler jwtTokenHandler)
            : base(commandDispatcher, validatorResolver)
        {
            Post("sign-in", async (ctx, p) => await For<SignIn>()
                .Set(c =>
                {
                    c.IpAddress = Request.UserHostAddress;
                    c.UserAgent = Request.Headers.UserAgent;
                })
                .SetResourceId(c => c.SessionId)
                .OnSuccess(async c =>
                {
                    var session = await userSessionProvider.GetAsync(c.SessionId);
                    var token = jwtTokenHandler.Create(session.Value.UserId);

                    return new {id = session.Value.Id, token = token, key = session.Value.Key};
                })
                .DispatchAsync());

            Post("sign-up", async (ctx, p) => await For<SignUp>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Post("sign-out", async (ctx, p) => await For<SignUp>()
                .OnSuccess(HttpStatusCode.NoContent)
                .DispatchAsync());
        }
    }
}