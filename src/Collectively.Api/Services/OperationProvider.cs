using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Operations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Collectively.Api.Services
{
    public class OperationProvider : IOperationProvider
    {
        private readonly IDistributedCache _cache;

        public OperationProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<Operation> GetAsync(Guid requestId)
        {
            var operation = await _cache.GetStringAsync($"operations:{requestId}");

            return operation == null ? null : JsonConvert.DeserializeObject<Operation>(operation);
        }
    }
}