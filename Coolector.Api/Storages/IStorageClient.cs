using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Queries;
using Coolector.Common.Types;

namespace Coolector.Api.Storages
{
    public interface IStorageClient
    {
        Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class;

        Task<Maybe<T>> GetUsingCacheAsync<T>(string endpoint, string cacheKey = null, TimeSpan? expiry = null)
            where T : class;

        Task<Maybe<Stream>> GetStreamAsync(string endpoint);

        Task<Maybe<PagedResult<T>>> GetCollectionUsingCacheAsync<T>(string endpoint, string cacheKey = null,
            TimeSpan? expiry = null) where T : class;

        Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TResult, TQuery>(TQuery query,
            string endpoint) where TResult : class where TQuery : class, IPagedQuery;

        Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionUsingCacheAsync<TResult, TQuery>(TQuery query,
            string endpoint, string cacheKey = null, TimeSpan? expiry = null)
            where TResult : class where TQuery : class, IPagedQuery;
    }
}