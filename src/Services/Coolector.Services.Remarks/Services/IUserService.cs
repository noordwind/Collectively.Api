using System.Threading.Tasks;

namespace Coolector.Services.Remarks.Services
{
    public interface IUserService
    {
        Task CreateAsync(string userId, string name);
    }
}