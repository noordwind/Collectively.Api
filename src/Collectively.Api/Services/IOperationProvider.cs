using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Services
{
    public interface IOperationProvider
    {
         Task<Operation> GetAsync(Guid requestId);
    }
}