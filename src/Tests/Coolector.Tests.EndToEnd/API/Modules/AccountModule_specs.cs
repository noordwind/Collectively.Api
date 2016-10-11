using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Dto.Users;
using Coolector.Tests.EndToEnd.Framework;
using Machine.Specifications;

namespace Coolector.Tests.EndToEnd.API.Modules
{
    public abstract class AccountModule_specs : ModuleBase_specs
    {
        protected static HttpResponseMessage ApiSignInResponse;

        protected static void Initialize()
        {
        }

        protected static async Task SignInAsync()
        {
            await SignInToAuth0Async();
            ApiSignInResponse = await HttpClient.PostAsync("sign-in", new
            {
                AccessToken = Auth0SignInResponse.AccessToken
            });
        }
    }

    [Subject("Auth0 sign in")]
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

    [Subject("Account sign in")]
    public class when_signing_in_to_api : AccountModule_specs
    {
        Establish context = () => Initialize();

        Because of = () => SignInAsync().GetAwaiter().GetResult();

        It should_return_successful_api_sign_in_response = () =>
        {
            ApiSignInResponse.IsSuccessStatusCode.ShouldBeTrue();
        };
    }

    [Subject("Account fetch")]
    public class when_fetching_account : AccountModule_specs
    {
        static UserDto User;

        Establish context = () => Initialize();

        Because of = () => User = RequestAuthenticatedAsync(c => c.GetAsync<UserDto>("account"))
            .GetAwaiter()
            .GetResult();

        It should_return_user_account = () =>
        {
            User.ShouldNotBeNull();
            User.Id.ShouldNotEqual(Guid.Empty);
            User.Name.ShouldNotBeEmpty();
            User.Role.ShouldNotBeEmpty();
            User.State.ShouldNotBeEmpty();
            User.CreatedAt.ShouldNotEqual(DateTime.UtcNow);
        };
    }
}