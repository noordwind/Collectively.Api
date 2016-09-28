using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;

namespace Coolector.Services.Users.Services
{
    public interface IUserService
    {
        Task<Maybe<UserDto>> GetAsync(string userId);
        Task CreateAsync(string userId, string email, Role role = Role.User, bool activate = true);
    }
}