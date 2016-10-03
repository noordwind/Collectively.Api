using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Dto.Users;

namespace Coolector.Services.Storage.Providers
{
    public interface IUserProvider
    {
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query); 
        Task<Maybe<UserDto>> GetAsync(string userId);
    }
}