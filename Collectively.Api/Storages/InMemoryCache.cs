using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Microsoft.Extensions.Caching.Memory;
using NLog;

namespace Collectively.Api.Storages
{
    public class InMemoryCache : ICache
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<Maybe<T>> GetAsync<T>(string key) where T : class
            => await Task.FromResult<Maybe<T>>(_cache.Get<T>(key));

        public async Task AddAsync(string key, object value, TimeSpan? expiry = null)
        {
            Logger.Debug($"Inserting item with key {key} to cache");
            if (expiry == null)
            {
                _cache.Set(key, value);
                await Task.CompletedTask;

                return;
            }

            _cache.Set(key, value, expiry.Value);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string key)
        {
            Logger.Debug($"Removing item with key {key} from cache");
            _cache.Remove(key);
            await Task.CompletedTask;
        }
    }
}