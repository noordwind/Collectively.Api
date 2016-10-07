using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;

namespace Coolector.Services.Remarks.Repositories
{
    public interface IRemarkRepository
    {
        Task<Maybe<Remark>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<string>> GetPhotoIdAsync(Guid id);
        Task AddAsync(Remark remark);
        Task UpdateAsync(Remark remark);
        Task DeleteAsync(Remark remark);
    }
}