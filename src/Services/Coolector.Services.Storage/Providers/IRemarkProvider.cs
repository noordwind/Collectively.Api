using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Providers
{
    public interface IRemarkProvider
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
    }
}