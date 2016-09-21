using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Coolector.Infrastructure.Mongo
{
    public static class Extensions
    {
        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase database, string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }

        public static async Task<IList<T>> GetAllAsync<T>(this IMongoCollection<T> collection)
            => await GetAllAsync(collection, _ => true);

        public static async Task<IList<T>> GetAllAsync<T>(this IMongoCollection<T> collection,
            Expression<Func<T, bool>> filter)
        {
            var filteredCollection = await collection.FindAsync(filter);
            var entities = await filteredCollection.ToListAsync();

            return entities;
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IMongoCollection<T> collection,
            Expression<Func<T, bool>> filter)
        {
            var filteredCollection = await collection.FindAsync(filter);
            var entities = await filteredCollection.ToListAsync();
            var entity = entities.FirstOrDefault();

            return entity;
        }

        public static async Task CreateCollectionAsync<T>(this IMongoDatabase database, string collectionName)
            => await database.CreateCollectionAsync(collectionName);

        public static async Task DropCollectionAsync<T>(this IMongoDatabase database, string collectionName)
            => await database.DropCollectionAsync(collectionName);
    }
}