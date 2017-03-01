using System;
using System.Threading.Tasks;
using Collectively.Common.Types;


namespace Collectively.Api.Storages
{
    public interface IOperationStorage
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
        Task<Maybe<OperationDto>> GetUpdatedAsync(Guid requestId);
    }
}