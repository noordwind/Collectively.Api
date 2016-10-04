using System;
using System.Threading.Tasks;
using Coolector.Services.Remarks.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Coolector.Services.Mongo;

namespace Coolector.Services.Remarks.Queries
{
    public static class RemarkQueries
    {
        public static IMongoCollection<Remark> Remarks(this IMongoDatabase database)
            => database.GetCollection<Remark>();

        public static async Task<Remark> GetByIdAsync(this IMongoCollection<Remark> remarks, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await remarks.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}