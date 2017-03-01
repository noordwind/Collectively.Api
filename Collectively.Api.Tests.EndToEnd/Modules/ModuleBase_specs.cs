using Collectively.Api.Tests.EndToEnd.Framework;
using System;
using System.Threading.Tasks;

namespace Collectively.Api.Tests.EndToEnd.Modules
{
    public abstract class ModuleBase_specs
    {
        private static bool _authenticated;
        protected static ApiSignInResponse ApiSignInResponse;
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:5000");
        protected static IOperationHandler OperationHandler = new OperationHandler(HttpClient);
        protected static string TestEmail = "test-e2e@noordwind.com";
        protected static string TestName = "test-e2e";
        protected static string TestPassword = "test";

        protected static void Initialize(bool authenticate = false)
        {
            if (authenticate)
                Authenticate();
        }

        protected static void SignInToApi()
            => ApiSignInResponse = GetApiSignInResponse();

        protected static ApiSignInResponse GetApiSignInResponse()
            => HttpClient.PostAsync<ApiSignInResponse>("sign-in", new
            {
                email = TestEmail,
                password = TestPassword,
                provider = "collectively"
            }).WaitForResult();

        protected static void Authenticate()
        {
            if (_authenticated)
                return;

            SignInToApi();
            HttpClient.SetHeader("Authorization", $"Bearer {ApiSignInResponse.Token}");
            _authenticated = true;
        }

        protected static void Wait(TimeSpan? timespan = null)
        {
            Task.Delay(timespan.GetValueOrDefault(TimeSpan.FromSeconds(1.0))).WaitForResult();
        }
    }
}