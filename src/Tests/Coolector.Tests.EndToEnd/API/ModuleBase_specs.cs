using System.Threading.Tasks;
using Coolector.Tests.EndToEnd.Framework;

namespace Coolector.Tests.EndToEnd.API
{
    public abstract class ModuleBase_specs
    {
        protected static Auth0SignInResponse Auth0SignInResponse;
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:5000");

        protected static IAuth0Client Auth0Client = new Auth0Client("noordwind-dev.eu.auth0.com",
            "eYnnpDd1k61vxXQCbFwWtX45yX3PxFDA");

        protected static void Initialize(bool authenticate = false)
        {
            if (authenticate)
                Authenticate();
        }

        protected static Task<Auth0SignInResponse> GetAuth0SignInResponseAsync()
            => Auth0Client.SignInAsync("noordwind-test1@mailinator.com", "test");

        protected static void SignInToAuth0()
            => Auth0SignInResponse = GetAuth0SignInResponseAsync().WaitForResult();

        protected static void Authenticate()
        {
            SignInToAuth0();
            HttpClient.SetHeader("Authorization", $"Bearer {Auth0SignInResponse.IdToken}");
        }
    }
}