using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Mappers;
using Newtonsoft.Json;

namespace Coolector.Services.Storage.Modules.Providers
{
    public class ServiceClient : IServiceClient
    {
        public async Task<Maybe<T>> GetAsync<T>(string url, string endpoint, IMapper<T> mapper) where T : class
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(GetBaseAddress(url)) };
            httpClient.DefaultRequestHeaders.Remove("Accept");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return new Maybe<T>();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(content);
            var result = mapper.Map(data);

            return result;
        }

        private string GetBaseAddress(string url) => url.EndsWith("/") ? url : $"{url}/";
    }
}