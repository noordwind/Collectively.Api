using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Services.Mongo;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Remarks.Repositories.Queries
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

        public static async Task<string> GetPhotoIdAsync(this IMongoCollection<Remark> remarks, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await remarks.AsQueryable().Where(x => x.Id == id)
                .Select(x => x.Photo.FileId)
                .FirstOrDefaultAsync(_ => true);
        }

        public static async Task<IEnumerable<Remark>> QueryAsync(this IMongoCollection<Remark> remarks,
            BrowseRemarks query)
        {
            if (Math.Abs(query.Latitude) <= 0.0000000001 || Math.Abs(query.Longitude) <= 0.0000000001 ||
                query.Radius <= 0)
            {
                return Enumerable.Empty<Remark>();
            }

            if (query.Page <= 0)
                query.Page = 1;
            if (query.Results <= 0)
                query.Results = 10;

            var filterBuilder = new FilterDefinitionBuilder<Remark>();
            var filter = FilterDefinition<Remark>.Empty;
            filter = filterBuilder.GeoWithinCenterSphere(x => x.Location,
                query.Longitude, query.Latitude, query.Radius/1000/6.3781);
            if (!query.Description.Empty())
                filter = filter & filterBuilder.Where(x => x.Description.Contains(query.Description));

            return await remarks.Find(filter)
                .SortBy(x => x.CreatedAt)
                .Skip(query.Results * (query.Page - 1))
                .Limit(query.Results)
                .ToListAsync();
        }
    }
}