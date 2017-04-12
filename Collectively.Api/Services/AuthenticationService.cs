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
        
        public async Task<Maybe<Session>> AuthenticateAsync(SignIn credentials)
        {
            if(HasInvalidCredentials(credentials))
            {
                return null;
            }

            var session = await _serviceClient.AuthenticateAsync(credentials);
            if(session.HasNoValue)
            {
                return null;
            }

            return new Session
            {
                Token = _jwtTokenHandler.Create(session.Value.UserId, TimeSpan.FromDays(_jwtTokenSettings.ExpiryDays)),
                SessionId = session.Value.Id,
                SessionKey = session.Value.Key,
                Expiry = DateTime.UtcNow.AddDays(_jwtTokenSettings.ExpiryDays).ToTimestamp()  
            };
        }

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
