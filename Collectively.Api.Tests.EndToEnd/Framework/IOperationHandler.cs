using System.Net.Http;
using System.Threading.Tasks;


namespace Collectively.Api.Tests.EndToEnd.Framework
{
    public interface IOperationHandler
    {
        Task<OperationDto> PostAsync(string endpoint, object data = null);
        Task<OperationDto> PutAsync(string endpoint, object data = null);
        Task<OperationDto> DeleteAsync(string endpoint);
        Task<OperationDto> HandleOperationAsync(HttpResponseMessage response);
        Task<OperationDto> HandleOperationAsync(string endpoint);
    }
}