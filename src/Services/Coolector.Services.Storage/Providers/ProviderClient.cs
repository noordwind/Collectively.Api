using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Mappers;

namespace Coolector.Services.Storage.Providers
{
    public class ProviderClient : IProviderClient
    {
        private readonly IServiceClient _serviceClient;

        public ProviderClient(IServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<T>> GetAsync<T>(string url, string endpoint, IMapper<T> mapper) where T : class
        => await _serviceClient.GetAsync<T>(url, endpoint, mapper);

        public async Task<Maybe<T>> GetUsingStorageAsync<T>(string url, string endpoint,
            Func<Task<Maybe<T>>> storageFetch, Func<T, Task> storageSave, IMapper<T> mapper) where T : class
        {
            if (storageFetch != null)
            {
                var data = await storageFetch();
                if (data.HasValue)
                    return data;
            }

            var response = await GetAsync(url, endpoint, mapper);
            if (response.HasNoValue)
                return new Maybe<T>();

            if (storageSave != null)
                await storageSave(response.Value);

            return response.Value;
        }
    }
}