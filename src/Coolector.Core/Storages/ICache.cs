using System;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Core.Storages
{
    public interface ICache
    {
        Task<Maybe<T>> GetAsync<T>(string key) where T : class;
        Task AddAsync(string key, object value, TimeSpan? expiry = null);
        Task DeleteAsync(string key);
    }
}