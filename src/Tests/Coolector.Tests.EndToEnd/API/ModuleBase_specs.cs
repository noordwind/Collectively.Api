using System.Threading.Tasks;
using Coolector.Tests.EndToEnd.Framework;

namespace Coolector.Tests.EndToEnd.API
{
    public abstract class ModuleBase_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:5000");

        protected static IAuth0Client Auth0Client = new Auth0Client("noordwind-dev.eu.auth0.com",
            "eYnnpDd1k61vxXQCbFwWtX45yX3PxFDA");

        protected static Task<Auth0SignInResponse> GetAuth0SignInResponseAsync()
            => Auth0Client.SignInAsync("noordwind-test1@mailinator.com", "test");
    }
}