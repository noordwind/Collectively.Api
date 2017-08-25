using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Messages.Commands.Users;

namespace Collectively.Api.Services
{
    public interface IAuthenticationService
    {
         Task<Maybe<JwtSession>> AuthenticateAsync(SignIn command);
         Task<Maybe<JwtSession>> RefreshSessionAsync(RefreshUserSession command);
    }
}