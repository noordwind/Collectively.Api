using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Storage.Repositories
{
    public class RemarkRepository : IRemarkRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<RemarkDto>> GetByIdAsync(Guid id)
            => await _database.Remarks().GetByIdAsync(id);

        public async Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query)
        {
            var results = await _database.Remarks()
                .QueryAsync(query);

            return results.Paginate(query);
        }

        public async Task<Maybe<string>> GetPhotoIdAsync(Guid id)
            => await _database.Remarks().GetPhotoIdAsync(id);

        public async Task AddAsync(RemarkDto remark)
            => await _database.Remarks().InsertOneAsync(remark);

        public async Task UpdateAsync(RemarkDto remark)
            => await _database.Remarks().ReplaceOneAsync(x => x.Id == remark.Id, remark);

        public async Task AddManyAsync(IEnumerable<RemarkDto> remarks)
            => await _database.Remarks().InsertManyAsync(remarks);
    }
}