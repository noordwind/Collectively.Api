using System;
using System.Threading.Tasks;
using Coolector.Api.Queries;
using Coolector.Common.Dto.Remarks;
using Coolector.Common.Types;

namespace Coolector.Api.Storages
{
    public interface IRemarkStorage
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
    }
}