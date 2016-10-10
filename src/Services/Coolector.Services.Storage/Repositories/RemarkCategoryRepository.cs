using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories.Queries;
using MongoDB.Driver;
using Coolector.Services.Mongo;

namespace Coolector.Services.Storage.Repositories
{
    public class RemarkCategoryRepository : IRemarkCategoryRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkCategoryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<RemarkCategoryDto>> GetByIdAsync(Guid id)
            => await _database.RemarkCategories().GetByIdAsync(id);

        public async Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseAsync(BrowseRemarkCategories query)
            => await _database.RemarkCategories()
                .Query(query)
                .PaginateAsync();

        public async Task AddManyAsync(IEnumerable<RemarkCategoryDto> remarks)
            => await _database.RemarkCategories().InsertManyAsync(remarks);
    }
}