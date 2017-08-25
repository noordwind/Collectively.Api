using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients;
using Collectively.Common.Types;
using Collectively.Messages.Commands.Users;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Services
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly IServiceClient _serviceClient;
        private readonly string _name;

        public UserServiceClient(IServiceClient serviceClient, string name)
        {
            _serviceClient = serviceClient;
            _name = name;
        }

        public async Task<Maybe<JwtSession>> AuthenticateAsync(SignIn command)
            => await _serviceClient.PostAsync<JwtSession>(_name, "sign-in", command);

        public async Task<Maybe<JwtSession>> RefreshSessionAsync(RefreshUserSession command)
            => await _serviceClient.PostAsync<JwtSession>(_name, "sessions", command);
    }
}