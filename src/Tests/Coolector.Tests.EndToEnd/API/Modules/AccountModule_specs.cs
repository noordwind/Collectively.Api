using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Tests.EndToEnd.Framework;
using Machine.Specifications;

namespace Coolector.Tests.EndToEnd.API.Modules
{
    public abstract class AccountModule_specs : ModuleBase_specs
    {
        protected static Auth0SignInResponse Auth0SignInResponse;
        protected static HttpResponseMessage ApiSignInResponse;

        protected static void Initialize()
        {
        }

        protected static async Task SignInToAuth0Async()
        {
            Auth0SignInResponse = await GetAuth0SignInResponseAsync();
        }

        protected static async Task SignInToApiAsync()
        {
            await SignInToAuth0Async();
            ApiSignInResponse = await HttpClient.PostAsync("sign-in", new
            {
                AccessToken = Auth0SignInResponse.AccessToken
            });
        }
    }

    [Subject("Account auth0 sign in")]
    public class when_signing_in_to_auth0 : AccountModule_specs
    {
        Establish context = () => Initialize();

        Because of = () => SignInToAuth0Async().GetAwaiter().GetResult();

        It should_return_successful_auth0_sign_in_response = () =>
        {
            Auth0SignInResponse.ShouldNotBeNull();
            Auth0SignInResponse.AccessToken.ShouldNotBeEmpty();
            Auth0SignInResponse.IdToken.ShouldNotBeEmpty();
            Auth0SignInResponse.TokenType.ShouldNotBeEmpty();
        };
    }

    [Subject("Account API sign in")]
    public class when_signing_in_to_api : AccountModule_specs
    {
        Establish context = () => Initialize();

        Because of = () =>
        {
            SignInToApiAsync().GetAwaiter().GetResult();
        };

        It should_return_successful_api_sign_in_response = () =>
        {
            ApiSignInResponse.IsSuccessStatusCode.ShouldBeTrue();
        };
    }
}