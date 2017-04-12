using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Messages.Commands.Users;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Services
{
    public interface IUserServiceClient
    {
        Task<Maybe<UserSession>> AuthenticateAsync(SignIn credentials);
    }
}