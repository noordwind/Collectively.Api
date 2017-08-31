using System;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Models.Operations;
using Newtonsoft.Json;

namespace Collectively.Api.Services
{
    public class OperationProvider : IOperationProvider
    {
        private readonly ICache _cache;

        public OperationProvider(ICache cache)
        {
            _cache = cache;
        }

        public async Task<Operation> GetAsync(Guid requestId)
        {
            var operation = await _cache.GetAsync<Operation>($"operations:{requestId}");

            return operation.HasValue ? operation.Value : null;
        }
    }
}