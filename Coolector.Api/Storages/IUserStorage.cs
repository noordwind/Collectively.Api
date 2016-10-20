using System.Threading.Tasks;
using Coolector.Api.Queries;
using Coolector.Common.Types;
using Coolector.Dto.Users;

namespace Coolector.Api.Storages
{
    public interface IUserStorage
    {
        Task<Maybe<UserDto>> GetAsync(string id);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
    }
}