using Coolector.Core.Domain.Remarks;
using MongoDB.Driver;

namespace Coolector.Core.Mongo.Queries
{
    public static class CategoryQueries
    {
        public static IMongoCollection<Category> Categories(this IMongoDatabase database)
            => database.GetCollection<Category>("Categories");
    }
}