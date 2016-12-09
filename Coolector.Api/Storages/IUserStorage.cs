using System;
using System.Threading.Tasks;
using Coolector.Api.Queries;
using Coolector.Common.Dto.General;
using Coolector.Common.Types;
using Coolector.Services.Users.Shared.Dto;

namespace Coolector.Api.Storages
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