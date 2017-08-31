using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using System.Security.Authentication;
using Collectively.Common.Caching;

namespace Collectively.Api.Services
{
    public class AccountStateProvider : IAccountStateProvider
    {
        private readonly ICache _cache;

        public AccountStateProvider(ICache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetAsync(string userId)
        {
            var state = await _cache.GetAsync<string>($"users:{userId}:state");

            return state.HasNoValue ? string.Empty : state.Value;
        }
    }
}