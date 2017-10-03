using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Storages
{
    public interface IOperationStorage
    {
        Task<Maybe<Operation>> GetAsync(Guid requestId);
    }
}