using System;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Services.Users.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Users.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<User> Users(this IMongoDatabase database)
            => database.GetCollection<User>("Users");


        public static async Task<User> GetByUserIdAsync(this IMongoCollection<User> users, string userId)
        {
            if (userId.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == userId);
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