using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Users.Domain;
using Coolector.Services.Users.Queries;

namespace Coolector.Services.Users.Repositories
{
    public interface IUserRepository
    {
        Task<Maybe<User>> GetByUserIdAsync(string userId);
        Task<Maybe<User>> GetByEmailAsync(string email);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}