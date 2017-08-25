using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Messages.Commands.Users;

namespace Collectively.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserServiceClient _serviceClient;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtTokenSettings _jwtTokenSettings;
        
        public AuthenticationService(IUserServiceClient serviceClient,
            IJwtTokenHandler jwtTokenHandler, JwtTokenSettings jwtTokenSettings)
        {
            _serviceClient = serviceClient;
            _jwtTokenHandler = jwtTokenHandler;
            _jwtTokenSettings = jwtTokenSettings;
        }
        
        public async Task<Maybe<JwtSession>> AuthenticateAsync(SignIn command)
        {
            if(HasInvalidCredentials(command))
            {
                return null;
            }
            
            return await _serviceClient.AuthenticateAsync(command);
        }

        public async Task<Maybe<JwtSession>> RefreshSessionAsync(RefreshUserSession command)
            => await _serviceClient.RefreshSessionAsync(command);

        private bool HasInvalidCredentials(SignIn credentials)
        {
            if(credentials == null || credentials.Provider.Empty())
            {
                return true;
            }
            if(credentials.Email.Empty() && credentials.Password.Empty() && credentials.AccessToken.Empty())
            {
                return true;
            }

            return false;
        }
    }
}
