using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Providers
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Remove("Accept");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<Maybe<HttpResponseMessage>> GetAsync(string url, string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(GetFullAddress(url, endpoint));
                if (response.IsSuccessStatusCode)
                    return response;
            }
            catch (Exception)
            {
            }

            return new Maybe<HttpResponseMessage>();
        }

        private string GetFullAddress(string url, string endpoint)
            => $"{(url.EndsWith("/", StringComparison.CurrentCultureIgnoreCase) ? url : $"{url}/")}{endpoint}";
    }
}