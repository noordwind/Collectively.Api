using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Core.Storages
{
    public interface IStorageClient
    {
        Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class;

        Task<Maybe<T>> GetUsingCacheAsync<T>(string endpoint, string cacheKey = null, TimeSpan? expiry = null)
            where T : class;

        Task<Maybe<IEnumerable<T>>> GetCollectionUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null) where T : class;
    }
}