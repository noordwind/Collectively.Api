using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Repositories
{
    public interface IRemarkRepository
    {
        Task<Maybe<RemarkDto>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task AddAsync(RemarkDto remark);
        Task UpdateAsync(RemarkDto remark);
        Task AddManyAsync(IEnumerable<RemarkDto> remarks);
    }
}