using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Humanizer;
using MongoDB.Driver;

namespace Coolector.Services.Mongo
{
    public static class Extensions
    {
        public static string GetCollectionName<T>(this IMongoCollection<T> collection) => Pluralize<T>();

        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase database)
        {
            var collectionName = Pluralize<T>();
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }

        private static string Pluralize<T>() => Pluralize(typeof(T));

        private static string Pluralize(Type type)
        {
            var typeName = type.Name;
            if (typeName.EndsWith("Dto"))
                typeName = typeName.Replace("Dto", string.Empty);

            var pluralizedName = typeName.Pluralize();

            return pluralizedName;
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

        public static async Task CreateCollectionAsync<T>(this IMongoDatabase database)
            => await database.CreateCollectionAsync(Pluralize<T>());

        public static async Task DropCollectionAsync<T>(this IMongoDatabase database)
            => await database.DropCollectionAsync(Pluralize<T>());
    }
}