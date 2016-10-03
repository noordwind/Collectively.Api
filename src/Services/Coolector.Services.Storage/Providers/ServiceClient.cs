using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Services.Storage.Mappers;
using Newtonsoft.Json;

namespace Coolector.Services.Storage.Providers
{
    public class ServiceClient : IServiceClient
    {
        private readonly IMapperResolver _mapperResolver;

        public ServiceClient(IMapperResolver mapperResolver)
        {
            _mapperResolver = mapperResolver;
        }

        public async Task<Maybe<T>> GetAsync<T>(string url, string endpoint) where T : class
        {
            var data = await GetDataAsync(url, endpoint);
            if (data.HasNoValue)
                return new Maybe<T>();
            
            var mapper = _mapperResolver.Resolve<T>();
            var result = mapper.Map(data);

            return result;
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string url, string endpoint) where T : class
        {
            var data = await GetDataAsync(url, endpoint);
            if (data.HasNoValue)
                return new Maybe<PagedResult<T>>();

            var mapper = _mapperResolver.ResolveForCollection<T>();
            var json = JsonConvert.SerializeObject(data.Value);
            var obj = JsonConvert.DeserializeObject<IEnumerable<object>>(json);
            IEnumerable<T> result = mapper.Map(obj);

            return result.PaginateWithoutLimit();
        }

        private async Task<Maybe<dynamic>> GetDataAsync(string url, string endpoint)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(GetBaseAddress(url)) };
            httpClient.DefaultRequestHeaders.Remove("Accept");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return new Maybe<dynamic>();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(content);

            return data;
        }

        private string GetBaseAddress(string url) => url.EndsWith("/") ? url : $"{url}/";
    }
}