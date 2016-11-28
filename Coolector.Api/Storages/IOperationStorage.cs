using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.Operations;
using Coolector.Common.Types;

namespace Coolector.Api.Storages
{
    public interface IOperationStorage
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}