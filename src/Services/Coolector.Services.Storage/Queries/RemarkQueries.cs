using System;
using System.Threading.Tasks;
using Coolector.Dto.Remarks;
using Coolector.Services.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Storage.Queries
{
    public static class RemarkQueries
    {
        public static IMongoCollection<RemarkDto> Remarks(this IMongoDatabase database)
            => database.GetCollection<RemarkDto>();

        public static async Task<RemarkDto> GetByIdAsync(this IMongoCollection<RemarkDto> remarks, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await remarks.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<string> GetPhotoIdAsync(this IMongoCollection<RemarkDto> remarks, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await remarks.AsQueryable().Where(x => x.Id == id)
                .Select(x => x.Photo.FileId)
                .FirstOrDefaultAsync(_ => true);
        }

        public static IMongoQueryable<RemarkDto> Query(this IMongoCollection<RemarkDto> remarks,
            BrowseRemarks query)
        {
            var values = remarks.AsQueryable();

            return values.OrderByDescending(x => x.CreatedAt);
        }
    }
}