using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Api.Storages
{
    public interface ICache
    {
        Task<Maybe<T>> GetAsync<T>(string key) where T : class;
        Task AddAsync(string key, object value, TimeSpan? expiry = null);
        Task DeleteAsync(string key);
    }
}