using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Repositories
{
    public interface ICategoryRepository
    {
        Task<Maybe<Category>> GetByIdAsync(Guid id);
        Task<Maybe<Category>> GetDefaultAsync();
        Task<Maybe<Category>> GetByNameAsync(string name);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
    }
}