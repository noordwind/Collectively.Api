using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Storages
{
    public interface IUserStorage
    {
        Task<Maybe<AvailableResource>> IsNameAvailableAsync(string name);
        Task<Maybe<User>> GetAsync(string id);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<UserSession>> GetSessionAsync(Guid id);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);
    }
}