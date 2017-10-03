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
    }
}