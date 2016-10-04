using System.Threading.Tasks;

namespace Coolector.Services.Remarks.Services
{
    public interface IUserService
    {
        Task CreateAsyncIfNotFound(string userId, string name);
    }
}