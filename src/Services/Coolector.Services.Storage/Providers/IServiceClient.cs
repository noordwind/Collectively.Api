using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Providers
{
    public interface IServiceClient
    {
        Task<Maybe<T>> GetAsync<T>(string url, string endpoint) where T : class;
    }
}