using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Providers
{
    public interface IUserProvider
    {
        Task<Maybe<UserDto>> GetAsync(string userId);
    }
}