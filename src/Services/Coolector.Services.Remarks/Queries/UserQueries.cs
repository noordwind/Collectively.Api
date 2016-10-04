using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Services.Remarks.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Coolector.Services.Mongo;

namespace Coolector.Services.Remarks.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<User> Users(this IMongoDatabase database)
            => database.GetCollection<User>();

        public static async Task<User> GetByUserIdAsync(this IMongoCollection<User> users, string userId)
        {
            if (userId.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public static async Task<User> GetByNameAsync(this IMongoCollection<User> users, string name)
        {
            if (name.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}