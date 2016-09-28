using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Core.Auth0;
using Coolector.Core.Services;

namespace Coolector.Core.Commands.Users
{
    public class SignInUserHandler : ICommandHandler<SignInUser>
    {
        private readonly IAuth0RestClient _auth0RestClient;
        private readonly IUserService _userService;

        public SignInUserHandler(IAuth0RestClient auth0RestClient, IUserService userService)
        {
            _auth0RestClient = auth0RestClient;
            _userService = userService;
        }

        public async Task HandleAsync(SignInUser command)
        {
            var user = await _auth0RestClient.GetUserByAccessTokenAsync(command.AccessToken);
            await _userService.SignInUserAsync(user.Email, user.UserId, user.Picture);
        }
    }
}