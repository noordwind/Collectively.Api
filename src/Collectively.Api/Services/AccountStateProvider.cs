using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Collectively.Common.Extensions;
using System.Security.Authentication;

namespace Collectively.Api.Services
{
    public class AccountStateProvider : IAccountStateProvider
    {
        private readonly IDistributedCache _cache;

        public AccountStateProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetAsync(string userId)
        => await _cache.GetStringAsync($"users:{userId}:state");
    }
}