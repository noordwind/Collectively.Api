using System;
using Collectively.Api.Commands;
using Collectively.Api.Services;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Users;
using Nancy;

namespace Collectively.Api.Modules
{
    public class AuthenticationModule : ModuleBase
    {
        public AuthenticationModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IAuthenticationService authenticationService)
            : base(commandDispatcher, validatorResolver)
        {
            Post("sign-in", async args => 
            {
                var credentials = BindRequest<SignIn>();
                credentials.Request = CreateRequest<SignIn>();
                credentials.SessionId = Guid.NewGuid();
                credentials.IpAddress = Request.UserHostAddress;
                credentials.UserAgent = Request.Headers.UserAgent;
                var session = await authenticationService.AuthenticateAsync(credentials);
                if (session.HasNoValue)
                {
                    return HttpStatusCode.Unauthorized;
                }

                return session.Value;
            });

            Post("sign-out", async (ctx, p) => await For<SignOut>()
                .OnSuccess(HttpStatusCode.NoContent)
                .DispatchAsync());
        }
    }
}