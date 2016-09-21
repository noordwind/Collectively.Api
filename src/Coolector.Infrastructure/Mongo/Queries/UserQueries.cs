using System;
using System.Threading.Tasks;
using Coolector.Core.Domain.Users;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Coolector.Core.Extensions;

namespace Coolector.Infrastructure.Mongo.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<User> Users(this IMongoDatabase database)
            => database.GetCollection<User>("Users");

        public static async Task<User> GetByIdAsync(this IMongoCollection<User> users,
            Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<User> GetByExternalIdAsync(this IMongoCollection<User> users, string externalId)
        {
            if (externalId.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.ExternalId == externalId);
        }

        public static async Task<User> GetByEmailAsync(this IMongoCollection<User> users, string email)
        {
            if (email.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<User> GetByNameAsync(this IMongoCollection<User> users, string name)
        {
            if (name.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}