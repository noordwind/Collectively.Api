using System;
using Coolector.Api.Commands;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using Coolector.Common.Extensions;
using Coolector.Common.Security;
using Coolector.Services.Users.Shared.Commands;
using Nancy;

namespace Coolector.Api.Modules
{
    public class AuthenticationModule : ModuleBase
    {
        public AuthenticationModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IUserStorage userStorage,
            IOperationStorage operationStorage,
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
                    var operation = await operationStorage.GetUpdatedAsync(c.Request.Id);
                    if(operation.HasNoValue || !operation.Value.Success)
                    {
                        return HttpStatusCode.Unauthorized;
                    }

                    var session = await userStorage.GetSessionAsync(c.SessionId);
                    if (session.HasNoValue)
                    {
                        return HttpStatusCode.Unauthorized;
                    }

                    return new
                    {
                        token = jwtTokenHandler.Create(session.Value.UserId),
                        sessionId = session.Value.Id,
                        sessionKey = session.Value.Key,
                        expiry = DateTime.UtcNow.AddDays(jwtTokenSettings.ExpiryDays).ToTimestamp()
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
    }
}