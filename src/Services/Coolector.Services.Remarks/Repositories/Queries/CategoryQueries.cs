using System;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Services.Mongo;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Remarks.Repositories.Queries
{
    public static class CategoryQueries
    {
        public static IMongoCollection<Category> Categories(this IMongoDatabase database)
            => database.GetCollection<Category>();

        public static async Task<Category> GetByIdAsync(this IMongoCollection<Category> categories, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await categories.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<Category> GetByNameAsync(this IMongoCollection<Category> categories, string name)
        {
            if (name.Empty())
                return null;

            return await categories.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);
        }

        public static IMongoQueryable<Category> Query(this IMongoCollection<Category> categories,
            BrowseCategories query)
        {
            var values = categories.AsQueryable();

            return values;
        }
    }
}