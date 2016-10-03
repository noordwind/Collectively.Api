using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Providers
{
    public interface IUserProvider
    {
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query); 
        Task<Maybe<UserDto>> GetAsync(string userId);
    }
}