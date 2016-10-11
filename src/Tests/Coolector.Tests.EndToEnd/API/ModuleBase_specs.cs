using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Tests.EndToEnd.Framework;

namespace Coolector.Tests.EndToEnd.API
{
    public abstract class ModuleBase_specs
    {
        protected static Auth0SignInResponse Auth0SignInResponse;
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:5000");

        protected static IAuth0Client Auth0Client = new Auth0Client("noordwind-dev.eu.auth0.com",
            "eYnnpDd1k61vxXQCbFwWtX45yX3PxFDA");

        protected static Task<Auth0SignInResponse> GetAuth0SignInResponseAsync()
            => Auth0Client.SignInAsync("noordwind-test1@mailinator.com", "test");

        protected static async Task SignInToAuth0Async()
        {
            Auth0SignInResponse = await GetAuth0SignInResponseAsync();
        }

        protected static async Task<HttpResponseMessage> RequestAuthenticatedAsync(
                Func<IHttpClient, Task<HttpResponseMessage>> request)
            => await RequestAuthenticatedAsync<HttpResponseMessage>(request);

        protected static async Task<T> RequestAuthenticatedAsync<T>(Func<IHttpClient, Task<T>> request)
        {
            if (Auth0SignInResponse == null || Auth0SignInResponse.AccessToken.Empty())
                await SignInToAuth0Async();

            HttpClient.SetHeader("Authorization", $"Bearer {Auth0SignInResponse.IdToken}");

            return await request(HttpClient);
        }
    }
}