using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Repositories
{
    public interface IUserRepository
    {
        Task<Maybe<User>> GetByUserIdAsync(string userId);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}