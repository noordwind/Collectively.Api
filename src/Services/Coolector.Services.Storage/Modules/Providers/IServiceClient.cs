using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Mappers;

namespace Coolector.Services.Storage.Modules.Providers
{
    public interface IServiceClient
    {
        Task<Maybe<T>> GetAsync<T>(string url, string endpoint, IMapper<T> mapper) where T : class;
    }
}