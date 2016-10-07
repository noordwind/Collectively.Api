using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public interface IRemarkService
    {
        Task<Maybe<Remark>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<FileStreamInfo>> GetPhotoAsync(Guid id);

        Task CreateAsync(Guid id, string userId, Guid categoryId, File photo,
            Position position, string description = null);

        Task DeleteAsync(Guid id, string userId);
    }
}