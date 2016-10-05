using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Common.Extensions;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using Coolector.Core.Filters;

namespace Coolector.Core.Storages
{
    public class StorageClient : IStorageClient
    {
        private readonly ICache _cache;
        private readonly IFilterResolver _filterResolver;
        private readonly StorageSettings _settings;
        private readonly HttpClient _httpClient;

        private string BaseAddress
            => _settings.Url.EndsWith("/", StringComparison.CurrentCulture) ? _settings.Url : $"{_settings.Url}/";

        public StorageClient(ICache cache, IFilterResolver filterResolver, StorageSettings settings)
        {
            _cache = cache;
            _filterResolver = filterResolver;
            _settings = settings;
            _httpClient = new HttpClient {BaseAddress = new Uri(BaseAddress)};
            _httpClient.DefaultRequestHeaders.Remove("Accept");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class
        {
            var response = await GetResponseAsync(endpoint);
            if(response.HasNoValue)
                return new Maybe<T>();

            var content = await response.Value.Content.ReadAsStringAsync();
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

        public async Task<Maybe<Stream>> GetStreamAsync(string endpoint)
        {
            var response = await GetResponseAsync(endpoint);
            if (response.HasNoValue)
                return new Maybe<Stream>();

            return await response.Value.Content.ReadAsStreamAsync();
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null) where T : class
        {
            var results = await GetFromCacheAsync<IEnumerable<T>>(endpoint, cacheKey);
            if (results.HasValue && results.Value.Any())
                return results.Value.PaginateWithoutLimit();

            results = await GetAsync<IEnumerable<T>>(endpoint);
            if (results.HasNoValue || !results.Value.Any())
                return new Maybe<PagedResult<T>>();

            await StoreInCacheAsync(results, endpoint, cacheKey, expiry);

            return results.Value.PaginateWithoutLimit();
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionc<TResult, TQuery>(TQuery query,
            string endpoint) where TResult : class where TQuery : class, IPagedQuery
        {
            var results = await GetAsync<IEnumerable<TResult>>(GetEndpointWithQuery(endpoint, query));
            if (results.HasNoValue || !results.Value.Any())
                return PagedResult<TResult>.Empty;

            return results.Value.Paginate(query);
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionUsingCacheAsync<TResult, TQuery>(
            TQuery query, string endpoint, string cacheKey = null, TimeSpan? expiry = null) where TResult : class
            where TQuery : class, IPagedQuery
        {
            var filter = _filterResolver.Resolve<TResult, TQuery>();
            var results = await GetFromCacheAsync<IEnumerable<TResult>>(endpoint, cacheKey);
            if (results.HasValue && results.Value.Any())
                return FilterAndPaginateResults(filter, results, query);

            results = await GetAsync<IEnumerable<TResult>>(GetEndpointWithQuery(endpoint, query));
            if (results.HasNoValue || !results.Value.Any())
                return PagedResult<TResult>.Empty;

            await StoreInCacheAsync(results, endpoint, cacheKey, expiry);

            return FilterAndPaginateResults(filter, results, query);
        }

        private async Task<Maybe<HttpResponseMessage>> GetResponseAsync(string endpoint)
        {
            if (endpoint.Empty())
                throw new ArgumentException("Endpoint can not be empty.");

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                    return response;
            }
            catch (Exception)
            {
            }

            return new Maybe<HttpResponseMessage>();
        }

        private static Maybe<PagedResult<TResult>> FilterAndPaginateResults<TResult, TQuery>(
            IFilter<TResult, TQuery> filter,
            Maybe<IEnumerable<TResult>> results, TQuery query) where TQuery : class, IPagedQuery
        => filter.Filter(results.Value, query).Paginate(query);

        private static string GetEndpointWithQuery<T>(string endpoint, T query) where T : class, IQuery
        {
            if (query == null)
                return endpoint;

            var values = new List<string>();
            foreach (var property in query.GetType().GetProperties())
            {
                var value = property.GetValue(query, null);
                values.Add($"{property.Name.ToLowerInvariant()}={value}");
            }

            var endpointQuery = string.Join("&", values);
            return $"{endpoint}?{endpointQuery}";
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