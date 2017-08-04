using System.Threading.Tasks;

namespace Collectively.Api.Services
{
    public interface IAccountStateProvider
    {
         Task<string> GetAsync(string userId);
    }
}