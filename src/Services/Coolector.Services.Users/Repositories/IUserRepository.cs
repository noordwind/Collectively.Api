using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Domain;
using Coolector.Services.Users.Domain;

namespace Coolector.Services.Users.Repositories
{
    public interface IUserRepository
    {
        Task<Maybe<User>> GetByUserIdAsync(string userId);
        Task<Maybe<User>> GetByEmailAsync(string email);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<PagedResult<User>>> BrowseAsync(int page = 1, int results = 10);
        Task AddAsync(User user);
    }
}