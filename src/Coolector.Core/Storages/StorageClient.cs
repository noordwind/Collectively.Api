using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Common.Extensions;
using Newtonsoft.Json;
using System.Linq;

namespace Coolector.Core.Storages
{
    public class StorageClient : IStorageClient
    {
        private readonly ICache _cache;
        private readonly StorageSettings _settings;
        private readonly HttpClient _httpClient;

        private string BaseAddress
            => _settings.Url.EndsWith("/", StringComparison.CurrentCulture) ? _settings.Url : $"{_settings.Url}/";

        public StorageClient(ICache cache, StorageSettings settings)
        {
            _cache = cache;
            _settings = settings;
            _httpClient = new HttpClient {BaseAddress = new Uri(BaseAddress)};
            _httpClient.DefaultRequestHeaders.Remove("Accept");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class
        {
            if (endpoint.Empty())
                throw new ArgumentException("Endpoint can not be empty.");

            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                    return new Maybe<T>();
            }
            catch (Exception)
            {
                return new Maybe<T>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }

        public async Task<Maybe<T>> GetUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null)
            where T : class
        {
            var result = await GetFromCacheAsync<T>(endpoint, cacheKey);
            if (result.HasValue)
                return result;

            result = await GetAsync<T>(endpoint);
            if (result.HasNoValue)
                return new Maybe<T>();

            await StoreInCacheAsync(result, endpoint, cacheKey, expiry);

            return result;
        }

        public async Task<Maybe<IEnumerable<T>>> GetCollectionUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null) where T : class
        {
            var result = await GetFromCacheAsync<IEnumerable<T>>(endpoint, cacheKey);
            if (result.HasValue && result.Value.Any())
                return result;

            result = await GetAsync<IEnumerable<T>>(endpoint);
            if (result.HasNoValue || !result.Value.Any())
                return new Maybe<IEnumerable<T>>();

            await StoreInCacheAsync(result, endpoint, cacheKey, expiry);

            return result;
        }

        private async Task<Maybe<T>> GetFromCacheAsync<T>(string endpoint, string cacheKey = null) where T : class
        {
            if (endpoint.Empty())
                throw new ArgumentException("Endpoint can not be empty.");

            cacheKey = GetCacheKey(endpoint, cacheKey);
            var result = await _cache.GetAsync<T>(cacheKey);

            return result.HasValue ? result : new Maybe<T>();
        }

        private async Task StoreInCacheAsync<T>(Maybe<T> value, string endpoint, string cacheKey = null,
            TimeSpan? expiry = null) where T : class
        {
            if (endpoint.Empty())
                throw new ArgumentException("Endpoint can not be empty.");
            if (value.HasNoValue)
                return;

            cacheKey = GetCacheKey(endpoint, cacheKey);
            var cacheExpiry = expiry ?? _settings.CacheExpiry;
            await _cache.AddAsync(cacheKey, value.Value, cacheExpiry);
        }

        private static string GetCacheKey(string endpoint, string cacheKey)
            => cacheKey.Empty() ? endpoint.Replace("/", ":") : cacheKey;
    }
}