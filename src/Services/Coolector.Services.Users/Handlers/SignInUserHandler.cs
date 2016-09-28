using System;
using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Users;
using Coolector.Common.Events.Users;
using Coolector.Services.Users.Auth0;
using Coolector.Services.Users.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace Coolector.Services.Users.Handlers
{
    public class SignInUserHandler : ICommandHandler<SignInUser>
    {
        private readonly IUserService _userService;
        private readonly IAuth0RestClient _auth0RestClient;
        private readonly IBusClient _bus;

        public SignInUserHandler(IUserService userService,
            IAuth0RestClient auth0RestClient,
            IBusClient bus)
        {
            _userService = userService;
            _auth0RestClient = auth0RestClient;
            _bus = bus;
        }

        public async Task HandleAsync(SignInUser command)
        {
            var auth0User = await _auth0RestClient.GetUserByAccessTokenAsync(command.AccessToken);
            var user = await _userService.GetAsync(auth0User.UserId);
            var userId = string.Empty;
            if (user.HasNoValue)
            {
                await _userService.CreateAsync(auth0User.UserId, auth0User.Email);
                user = await _userService.GetAsync(auth0User.UserId);
                userId = user.Value.UserId;
                await _bus.PublishAsync(new UserCreated(userId, user.Value.Email,
                    user.Value.Role, user.Value.State, user.Value.CreatedAt));

                return;
            }
            userId = user.Value.UserId;
            await _bus.PublishAsync(new UserSignedIn(userId, user.Value.Email));
        }
    }
}