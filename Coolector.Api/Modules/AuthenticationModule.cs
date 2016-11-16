using System;
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
            IJwtTokenHandler jwtTokenHandler,
            JwtTokenSettings jwtTokenSettings)
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
                    if (session.HasNoValue)
                        return HttpStatusCode.Unauthorized;

                    return new
                    {
                        token = jwtTokenHandler.Create(session.Value.UserId),
                        sessionId = session.Value.Id,
                        key = session.Value.Key,
                        expiry = ToJavascriptTimestamp(DateTime.UtcNow.AddDays(jwtTokenSettings.ExpiryDays))
                    };
                })
                .DispatchAsync());

            Post("sign-up", async (ctx, p) => await For<SignUp>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Post("sign-out", async (ctx, p) => await For<SignUp>()
                .OnSuccess(HttpStatusCode.NoContent)
                .DispatchAsync());
        }

        private static long ToJavascriptTimestamp(DateTime input)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var time = input.Subtract(new TimeSpan(epoch.Ticks));

            return time.Ticks / 10000;
        }
    }
}