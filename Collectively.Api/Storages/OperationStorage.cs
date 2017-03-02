using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Storages
{
    public class OperationStorage : IOperationStorage
    {
        private readonly IStorageClient _storageClient;

        public OperationStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
            => await _storageClient.GetAsync<Operation>($"operations/{requestId}");

        public async Task<Maybe<Operation>> GetUpdatedAsync(Guid requestId)
        {
            var requestsCount = 0;
            var operation = await GetAsync(requestId);
            while((operation.HasNoValue || operation.Value.State == "created") && requestsCount < 10) 
            {
                operation = await GetAsync(requestId);
                requestsCount++;
                await Task.Delay(500);
            }

            return operation;
        }
    }
}