using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Operations;

namespace Coolector.Api.Storages
{
    public interface IOperationStorage
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}