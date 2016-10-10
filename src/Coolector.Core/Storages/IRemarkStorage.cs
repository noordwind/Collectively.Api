using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Core.Queries;
using Coolector.Dto.Remarks;

namespace Coolector.Core.Storages
{
    public interface IRemarkStorage
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<Stream>> GetPhotoAsync(Guid id);
    }
}