using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;

namespace Coolector.Services.Remarks.Repositories
{
    public interface ICategoryRepository
    {
        Task<Maybe<Category>> GetByIdAsync(Guid id);
        Task<Maybe<Category>> GetByNameAsync(string name);
        Task<Maybe<PagedResult<Category>>> BrowseAsync(BrowseCategories query);
        Task AddManyAsync(IEnumerable<Category> remarks);
    }
}