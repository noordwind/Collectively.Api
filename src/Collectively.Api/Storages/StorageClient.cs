using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Collectively.Api.Filters;
using Collectively.Common.Queries;
using Collectively.Common.Types;
using Collectively.Common.Extensions;
using System.Linq;
using Serilog;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients;

namespace Collectively.Api.Storages
{
    public class StorageClient : IStorageClient
    {
        private static readonly ILogger Logger = Log.Logger;
        private readonly IServiceClient _serviceClient;
        private readonly IMemoryCache _cache;
        private readonly IFilterResolver _filterResolver;
        private readonly IServiceAuthenticatorClient _serviceAuthenticatorClient;
        private readonly ServiceSettings _settings;

        public StorageClient(IServiceClient serviceClient, IMemoryCache cache, 
            IFilterResolver filterResolver, IServiceAuthenticatorClient serviceAuthenticatorClient,
            ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _cache = cache;
            _filterResolver = filterResolver;
            _serviceAuthenticatorClient = serviceAuthenticatorClient;
            _settings = settings;
        }

        public async Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class
        {
            Logger.Debug($"Get data from storage, endpoint: {endpoint}");

            return await _serviceClient.GetAsync<T>(_settings.Name, endpoint);
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string endpoint) where T : class
        {
            Logger.Debug($"Get data collection from storage, endpoint: {endpoint}");

            return await _serviceClient.GetCollectionAsync<T>(_settings.Name, endpoint);
        }

        public async Task<Maybe<T>> GetUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null)
            where T : class
        {
            Logger.Debug($"Get data from cache... endpoint: {endpoint}, cacheKey: {cacheKey}");
            var result = await GetFromCacheAsync<T>(endpoint, cacheKey);
            if (result.HasValue)
                return result;

            Logger.Debug($"No data in cache, try get data from storage... endpoint: {endpoint}");
            result = await GetAsync<T>(endpoint);
            if (result.HasNoValue)
                return new Maybe<T>();

            Logger.Debug($"Store missing data in cache, endpoint: {endpoint}, cacheKey: {cacheKey}, expiry: {expiry}, type: {typeof(T).Name}");
            await StoreInCacheAsync(result, endpoint, cacheKey, expiry);

            return result;
        }

        public async Task<Maybe<Stream>> GetStreamAsync(string endpoint)
        {
            Logger.Debug($"Get stream from endpoint: {endpoint}");

            return await _serviceClient.GetStreamAsync(_settings.Name, endpoint);
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null) where T : class
        {
            Logger.Debug($"Get collection of data from cache... endpoint: {endpoint}, cacheKey: {cacheKey}");
            var results = await GetFromCacheAsync<IEnumerable<T>>(endpoint, cacheKey);
            if (results.HasValue && results.Value.Any())
            {
                return results.Value.PaginateWithoutLimit();
            }

            Logger.Debug($"No data in cache, try get collection of data from storage... endpoint: {endpoint}");
            results = await GetAsync<IEnumerable<T>>(endpoint);
            if (results.HasNoValue || !results.Value.Any())
            {
                return null;
            }
            Logger.Debug($"Store missing collection in cache, endpoint: {endpoint}, cacheKey: {cacheKey}, expiry: {expiry}, type: {typeof(T).Name}");
            await StoreInCacheAsync(results, endpoint, cacheKey, expiry);

            return results.Value.PaginateWithoutLimit();
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TResult, TQuery>(TQuery query,
            string endpoint) where TResult : class where TQuery : class, IPagedQuery
        {
            Logger.Debug($"Get filtered data from storage, endpoint: {endpoint}, queryType: {typeof(TQuery).Name}");
            var queryString = endpoint.ToQueryString(query);
            var results = await GetCollectionAsync<TResult>(queryString);
            if (results.HasNoValue || results.Value.IsEmpty)
                return PagedResult<TResult>.Empty;

            return results.Value;
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionUsingCacheAsync<TResult, TQuery>(
            TQuery query, string endpoint, string cacheKey = null, TimeSpan? expiry = null) where TResult : class
            where TQuery : class, IPagedQuery
        {
            Logger.Debug($"Get filtered collection of data from cache... endpoint: {endpoint}, cacheKey: {cacheKey}, queryType: {typeof(TQuery).Name}");
            var filter = _filterResolver.Resolve<TResult, TQuery>();
            var results = await GetFromCacheAsync<IEnumerable<TResult>>(endpoint, cacheKey);
            if (results.HasValue && results.Value.Any())
            {
                return FilterAndPaginateResults(filter, results, query);
            }

            Logger.Debug($"Get filtered collection of data from storage, endpoint: {endpoint}, queryType: {typeof(TQuery).Name}");
            var queryString = endpoint.ToQueryString(query);
            results = await GetAsync<IEnumerable<TResult>>(queryString);
            if (results.HasNoValue || !results.Value.Any())
            {
                return PagedResult<TResult>.Empty;
            }

            Logger.Debug($"Store missing collection in cache, endpoint: {endpoint}, cacheKey: {cacheKey}, expiry: {expiry}, type: {typeof(TResult).Name}");
            await StoreInCacheAsync(results, endpoint, cacheKey, expiry);

            return FilterAndPaginateResults(filter, results, query);
        }

        private static Maybe<PagedResult<TResult>> FilterAndPaginateResults<TResult, TQuery>(
            IFilter<TResult, TQuery> filter,
            Maybe<IEnumerable<TResult>> results, TQuery query) where TQuery : class, IPagedQuery
            => filter.Filter(results.Value, query).Paginate(query);

        private async Task<Maybe<T>> GetFromCacheAsync<T>(string endpoint, string cacheKey = null) where T : class
        {
            if (endpoint.Empty())
                throw new ArgumentException("Endpoint can not be empty.");

            Logger.Debug($"Fetch data from cache, type: {typeof(T).Name}, endpoint: {endpoint}, cacheKey: {cacheKey}");
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
            Logger.Debug($"Store data in cache, type: {typeof(T).Name}, endpoint: {endpoint}, cacheKey: {cacheKey}, expiry: {expiry}");
            await _cache.AddAsync(cacheKey, value.Value, cacheExpiry);
        }

        private static string GetCacheKey(string endpoint, string cacheKey)
            => cacheKey.Empty() ? endpoint.Replace("/", ":") : cacheKey;
    }
}