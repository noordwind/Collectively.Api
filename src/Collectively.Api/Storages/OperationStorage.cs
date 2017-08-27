using System;
using System.Threading.Tasks;
using Collectively.Api.Services;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Storages
{
    public class OperationStorage : IOperationStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly IOperationProvider _operationProvider;

        public OperationStorage(IStorageClient storageClient, 
            IOperationProvider operationProvider)
        {
            _storageClient = storageClient;
            _operationProvider = operationProvider;
        }

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
            => await _operationProvider.GetAsync(requestId);

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