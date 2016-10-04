using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Dto.Users;

namespace Coolector.Services.Storage.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExisitsAsync(string id);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<UserDto>> GetByIdAsync(string id);
        Task EditAsync(UserDto user);
        Task AddAsync(UserDto user);
        Task AddManyAsync(IEnumerable<UserDto> users);
    }
}