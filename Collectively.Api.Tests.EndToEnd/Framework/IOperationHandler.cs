using System.Net.Http;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Tests.EndToEnd.Framework
{
    public interface IOperationHandler
    {
        Task<Operation> PostAsync(string endpoint, object data = null);
        Task<Operation> PutAsync(string endpoint, object data = null);
        Task<Operation> DeleteAsync(string endpoint);
        Task<Operation> HandleOperationAsync(HttpResponseMessage response);
        Task<Operation> HandleOperationAsync(string endpoint);
    }
}