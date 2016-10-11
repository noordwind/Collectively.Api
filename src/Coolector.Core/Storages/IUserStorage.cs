using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Core.Filters;
using Coolector.Core.Queries;
using Coolector.Dto.Users;

namespace Coolector.Core.Storages
{
    public interface IUserStorage
    {
        Task<Maybe<UserDto>> GetAsync(string id);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
    }
}