using Coolector.Api.Tests.EndToEnd.Framework;
using Machine.Specifications;
using System;
using Coolector.Common.Dto.Users;

namespace Coolector.Api.Tests.EndToEnd.Modules
{
    public abstract class AccountModule_specs : ModuleBase_specs
    {
        protected static UserDto GetAccount()
            => HttpClient.GetAsync<UserDto>("account").WaitForResult();

        protected static UserDto GetAccountByName(string name)
            => HttpClient.GetAsync<UserDto>($"users/{name}").WaitForResult();
    }

    [Subject("Account sign in")]
    public class when_signing_in_to_api : AccountModule_specs
    {

        Establish context = () => Initialize(authenticate: true);

        Because of = () => GetApiSignInResponse();

        It should_return_token = () => ApiSignInResponse.Token.ShouldNotBeEmpty();
        It should_return_session_id = () => ApiSignInResponse.SessionId.ShouldNotBeEmpty();
        It should_return_session_key = () => ApiSignInResponse.SessionKey.ShouldNotBeEmpty();
        It should_return_expiry = () => ApiSignInResponse.Expiry.ShouldBeGreaterThan(0);
    }

    [Subject("Account fetch")]
    public class when_fetching_account : AccountModule_specs
    {
        static UserDto User;

        Establish context = () => Initialize(authenticate: true);

        Because of = () => User = GetAccount();

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

    [Subject("Account fetch by name")]
    public class when_fetching_account_by_name : AccountModule_specs
    {
        static UserDto User;

        Establish context = () => Initialize(true);

        Because of = () => User = GetAccountByName(TestName);

        It should_return_user_account = () =>
        {
            User.ShouldNotBeNull();
            User.Id.ShouldNotEqual(Guid.Empty);
            User.Name.ShouldEqual(TestName);
            User.Role.ShouldNotBeEmpty();
            User.State.ShouldNotBeEmpty();
            User.CreatedAt.ShouldNotEqual(DateTime.UtcNow);
        };
    }
}