using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Infrastructure.DTO.Users;
using Coolector.Infrastructure.Settings;
using Newtonsoft.Json;

namespace Coolector.Infrastructure.Auth0
{
    public interface IAuth0RestClient
    {
        Task<Auth0User> GetUserByAccessTokenAsync(string accessToken);
    }

    public class Auth0RestClient : IAuth0RestClient
    {
        private readonly Auth0Settings _settings;
        private readonly HttpClient _httpClient;

        private readonly string AuthorizationHeader = "Authorization";
        private string BaseAddress => $"https://{_settings.Domain}/";
        
        public Auth0RestClient(Auth0Settings settings)
        {
            _settings = settings;
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };
        }

        public async Task<Auth0User> GetUserByAccessTokenAsync(string accessToken)
            => await GetUserAsync("userinfo", accessToken);

        private async Task<Auth0User> GetUserAsync(string endpoint, string token)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(AuthorizationHeader))
                _httpClient.DefaultRequestHeaders.Remove(AuthorizationHeader);

            _httpClient.DefaultRequestHeaders.Add(AuthorizationHeader, $"Bearer {token}");
            var response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Auth0User>(content);

            return user;
        }
    }
}