using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Api.Storages
{
    public interface IRemarkStorage
    {
        Task<Maybe<Remark>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<PagedResult<Remark>>> BrowseSimilarAsync(BrowseSimilarRemarks query);
        Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}