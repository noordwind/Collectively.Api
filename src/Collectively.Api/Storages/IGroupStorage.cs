using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Api.Storages
{
    public interface IGroupStorage
    {
        Task<Maybe<Group>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query);         
    }
}