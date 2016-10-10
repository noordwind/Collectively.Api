using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Users.Domain;
using Coolector.Services.Users.Queries;

namespace Coolector.Services.Users.Services
{
    public interface IUserService
    {
        Task<Maybe<User>> GetAsync(string userId);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);

        Task CreateAsync(string userId, string email, string role, bool activate = true, string pictureUrl = null,
            string name = null);

        Task ChangeNameAsync(string userId, string name);
        Task ChangeAvatarAsync(string userId, string name);
    }
}