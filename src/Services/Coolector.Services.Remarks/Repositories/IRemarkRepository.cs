using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Repositories
{
    public interface IRemarkRepository
    {
        Task<Maybe<Remark>> GetByIdAsync(Guid id);
        Task AddAsync(Remark remark);
        Task UpdateAsync(Remark remark);
    }
}