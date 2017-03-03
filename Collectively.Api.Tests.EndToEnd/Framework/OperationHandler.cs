using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Tests.EndToEnd.Framework
{
    public class OperationHandler : IOperationHandler
    {
        private readonly IHttpClient _httpClient;

        public OperationHandler(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Operation> PostAsync(string endpoint, object data = null)
            => await HandleOperationAsync(await _httpClient.PostAsync(endpoint, data));

        public async Task<Operation> PutAsync(string endpoint, object data = null)
            => await HandleOperationAsync(await _httpClient.PutAsync(endpoint, data));

        public async Task<Operation> DeleteAsync(string endpoint)
            => await HandleOperationAsync(await _httpClient.DeleteAsync(endpoint));

        public async Task<Operation> HandleOperationAsync(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new InvalidOperationException($"Invalid operation status code: {response.StatusCode}.");
            }
            var endpoint = response.Headers.GetValues("X-Operation").FirstOrDefault();
            if (endpoint.Empty())
            {
                throw new ArgumentException($"X-Operation header has no value.", nameof(endpoint));
            }

            return await HandleOperationAsync(endpoint);
        }

        public async Task<Operation> HandleOperationAsync(string endpoint)
        {
            var retryNumber = 0;
            while (retryNumber < 10)
            {
                var operation = await _httpClient.GetAsync<Operation>(endpoint);
                if (operation != null && operation.State != "created")
                    return operation;

                await Task.Delay(1000);
                retryNumber++;
            }

            throw new InvalidOperationException($"Could not complete operation: {endpoint}.");
        }
    }
}