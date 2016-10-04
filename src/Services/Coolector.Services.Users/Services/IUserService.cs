using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Users.Domain;

namespace Coolector.Services.Users.Services
{
    public interface IUserService
    {
        Task<Maybe<User>> GetAsync(string userId);
        Task<Maybe<PagedResult<User>>> BrowseAsync(int page = 1, int results = 10);

        Task CreateAsync(string userId, string email, string role, bool activate = true, string pictureUrl = null,
            string name = null);

        Task ChangeNameAsync(string userId, string name);
    }
}