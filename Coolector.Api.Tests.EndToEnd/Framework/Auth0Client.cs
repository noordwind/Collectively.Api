using System.Threading.Tasks;

namespace Coolector.Api.Tests.EndToEnd.Framework
{
    public class Auth0Client : IAuth0Client
    {
        private readonly string _domain;
        private readonly string _clientId;
        private readonly IHttpClient _httpClient;

        public Auth0Client(string domain, string clientId)
        {
            _domain = domain;
            _clientId = clientId;
            _httpClient = new CustomHttpClient($"https://{domain}");
        }

        public async Task<Auth0SignInResponse> SignInAsync(string username, string password)
        {
            var data = new
            {
                client_id = _clientId,
                username,
                password,
                connection = "Username-Password-Authentication",
                grant_type = "password",
                scope = "openid"
            };

            return await _httpClient.PostAsync<Auth0SignInResponse>("oauth/ro", data);
        }
    }
}