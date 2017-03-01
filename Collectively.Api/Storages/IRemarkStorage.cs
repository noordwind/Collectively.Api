using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;


namespace Collectively.Api.Storages
{
    public interface IRemarkStorage
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<TagDto>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}