using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;


namespace Collectively.Api.Storages
{
    public interface IUserStorage
    {
        Task<Maybe<AvailableResourceDto>> IsNameAvailableAsync(string name);
        Task<Maybe<UserDto>> GetAsync(string id);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
    }
}