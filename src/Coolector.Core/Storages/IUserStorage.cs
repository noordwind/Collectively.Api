using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Core.Filters;

namespace Coolector.Core.Storages
{
    public interface IUserStorage
    {
        Task<Maybe<UserDto>> GetAsync(string id);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
    }
}