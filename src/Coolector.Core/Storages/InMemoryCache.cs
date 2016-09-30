using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Microsoft.Extensions.Caching.Memory;

namespace Coolector.Core.Storages
{
    public class InMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<Maybe<T>> GetAsync<T>(string key) where T : class
        => await Task.FromResult<Maybe<T>>(_cache.Get<T>(key));

        public async Task AddAsync(string key, object value, TimeSpan? expiry = null)
        {
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
            _cache.Remove(key);
            await Task.CompletedTask;
        }
    }
}