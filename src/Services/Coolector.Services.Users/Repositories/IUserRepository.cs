using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Users.Domain;

namespace Coolector.Services.Users.Repositories
{
    public interface IUserRepository
    {
        Task<Maybe<User>> GetByUserIdAsync(string userId);
        Task<Maybe<User>> GetByEmailAsync(string email);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task AddAsync(User user);
    }
}