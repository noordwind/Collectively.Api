using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Dto.Operations;
using Coolector.Common.Extensions;

namespace Coolector.Api.Tests.EndToEnd.Framework
{
    public class OperationHandler : IOperationHandler
    {
        private readonly IHttpClient _httpClient;

        public OperationHandler(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperationDto> PostAsync(string endpoint, object data = null)
            => await HandleOperationAsync(await _httpClient.PostAsync(endpoint, data));

        public async Task<OperationDto> PutAsync(string endpoint, object data = null)
            => await HandleOperationAsync(await _httpClient.PutAsync(endpoint, data));

        public async Task<OperationDto> DeleteAsync(string endpoint)
            => await HandleOperationAsync(await _httpClient.DeleteAsync(endpoint));

        public async Task<OperationDto> HandleOperationAsync(HttpResponseMessage response)
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

        public async Task<OperationDto> HandleOperationAsync(string endpoint)
        {
            var retryNumber = 0;
            while (retryNumber < 10)
            {
                var operation = await _httpClient.GetAsync<OperationDto>(endpoint);
                if (operation != null && operation.State != "created")
                    return operation;

                await Task.Delay(1000);
                retryNumber++;
            }

            throw new InvalidOperationException($"Could not complete operation: {endpoint}.");
        }
    }
}