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
                var command = BindRequest<SignIn>();
                command.Request = CreateRequest<SignIn>();
                command.SessionId = Guid.NewGuid();
                command.IpAddress = Request.UserHostAddress;
                command.UserAgent = Request.Headers.UserAgent;
                var session = await authenticationService.AuthenticateAsync(command);
                if (session.HasNoValue)
                {
                    return HttpStatusCode.Unauthorized;
                }

                return session.Value;
            });

            Post("sign-out", async (ctx, p) => await For<SignOut>()
                .OnSuccess(HttpStatusCode.NoContent)
                .DispatchAsync());

            Post("sessions", async args => 
            {
                var command = BindRequest<RefreshUserSession>();
                command.Request = CreateRequest<RefreshUserSession>();
                command.NewSessionId = Guid.NewGuid();
                var session = await authenticationService.RefreshSessionAsync(command);
                if (session.HasNoValue)
                {
                    return HttpStatusCode.Forbidden;
                }

                return session.Value;
            });
        }
    }
}