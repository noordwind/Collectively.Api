using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Mongo;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
using Coolector.Services.Remarks.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Remarks.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDatabase _database;

        public CategoryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<Category>> GetByIdAsync(Guid id)
            => await _database.Categories().GetByIdAsync(id);

        public async Task<Maybe<Category>> GetByNameAsync(string name)
            => await _database.Categories().GetByNameAsync(name);

        public async Task<Maybe<PagedResult<Category>>> BrowseAsync(BrowseCategories query)
            => await _database.Categories()
                .Query(query)
                .PaginateAsync();

        public async Task AddManyAsync(IEnumerable<Category> remarks)
            => await _database.Categories().InsertManyAsync(remarks);
    }
}