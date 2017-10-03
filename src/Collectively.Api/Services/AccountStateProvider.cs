using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Caching;
using Collectively.Api.Storages;
using Microsoft.Extensions.Caching.Memory;

namespace Collectively.Api.Services
{
    public class AccountStateProvider : IAccountStateProvider
    {
        private readonly ICache _redisCache;
        private readonly IMemoryCache _memoryCache;
        private readonly IStorageClient _storageClient;

        public AccountStateProvider(
            IStorageClient storageClient,
            IMemoryCache memoryCache,
            ICache redisCache)
        {
            _storageClient = storageClient;
            _memoryCache = memoryCache;
            _redisCache = redisCache;
        }

        //Fetch user state from Redis or Memory Cache if not found.
        //Otherwise, fetch from Storage Service and save into the Memory Cache.
        public async Task<string> GetAsync(string userId)
        {
            var state = await _redisCache.GetAsync<string>(GetCacheKey(userId));
            if (state.HasValue && state.Value.NotEmpty())
            {
                return state.Value;
            }
            var userState = _memoryCache.Get<string>(GetCacheKey(userId));
            if (userState.NotEmpty())
            {
                return userState;
            }
            state = await _storageClient.GetAsync<string>($"users/{userId}/state");
            if (state.HasValue)
            {
                _memoryCache.Set(GetCacheKey(userId), state.Value);

                return state.Value;
            }

            return string.Empty;
        }

        private static string GetCacheKey(string userId)
            => $"users:{userId}:state";
    }
}