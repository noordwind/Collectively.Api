using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
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

        public async Task AddAsync(Category category)
            => await _database.Categories().InsertOneAsync(category);

        public async Task UpdateAsync(Category category)
            => await _database.Categories().ReplaceOneAsync(x => x.Id == category.Id, category);
    }
}