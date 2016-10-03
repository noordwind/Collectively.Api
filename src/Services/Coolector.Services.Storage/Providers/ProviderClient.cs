using System;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Providers
{
    public class ProviderClient : IProviderClient
    {
        private readonly IServiceClient _serviceClient;

        public ProviderClient(IServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<T>> GetAsync<T>(string url, string endpoint) where T : class
        => await _serviceClient.GetAsync<T>(url, endpoint);

        public async Task<Maybe<T>> GetUsingStorageAsync<T>(string url, string endpoint,
            Func<Task<Maybe<T>>> storageFetch, Func<T, Task> storageSave) where T : class
        {
            if (storageFetch != null)
            {
                var data = await storageFetch();
                if (data.HasValue)
                    return data;
            }

            var response = await GetAsync<T>(url, endpoint);
            if (response.HasNoValue)
                return new Maybe<T>();

            if (storageSave != null)
                await storageSave(response.Value);

            return response.Value;
        }

        public Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string url, string endpoint) where T : class
            => _serviceClient.GetCollectionAsync<T>(url, endpoint);

        public async Task<Maybe<PagedResult<T>>> GetCollectionUsingStorageAsync<T>(string url, string endpoint, 
            Func<Task<Maybe<PagedResult<T>>>> storageFetch, Func<PagedResult<T>, Task> storageSave) where T : class
        {
            if (storageFetch != null)
            {
                var data = await storageFetch();
                if (data.HasValue && data.Value.Items.Any())
                    return data;
            }

            var response = await GetCollectionAsync<T>(url, endpoint);
            if (response.HasNoValue)
                return response;

            if (storageSave != null)
                await storageSave(response.Value);

            return response;
        }
    }
}