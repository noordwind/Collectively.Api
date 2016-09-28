using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Newtonsoft.Json;

namespace Coolector.Services.Users.Auth0
{
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

        public async Task<Auth0UserDto> GetUserAsync(string userId)
            => await GetUserAsync($"api/v2/users/{userId}", _settings.ReadUsersToken);

        public async Task<Auth0UserDto> GetUserByAccessTokenAsync(string accessToken)
            => await GetUserAsync("userinfo", accessToken);

        private async Task<Auth0UserDto> GetUserAsync(string endpoint, string token)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(AuthorizationHeader))
                _httpClient.DefaultRequestHeaders.Remove(AuthorizationHeader);

            _httpClient.DefaultRequestHeaders.Add(AuthorizationHeader, $"Bearer {token}");
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return new Auth0UserDto();

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Auth0UserDto>(content);

            return user;
        }
    }
}