using System;
using System.Collections.Generic;
using System.Linq;
using Coolector.Services.Users.Domain;
using Coolector.Tests.EndToEnd.Framework;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Coolector.Tests.EndToEnd.Services.Users
{
    public abstract class UserModule_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:10001");
        protected static User User;
        protected static IEnumerable<User> Users;
        protected static string UserId;
        protected static string UserName;

        protected static void Initialize()
        {
        }

        protected static void InitializeAndFetchUsers()
        {
            Initialize();
            var users = FetchUsers();
            var user = users.First();
            UserId = user.UserId;
            UserName = user.Name;
        }

        protected static User FetchUser(string id)
            => HttpClient.GetAsync<User>($"users/{id}").WaitForResult();

        protected static User FetchUserByName(string name)
            => HttpClient.GetAsync<User>($"users/{name}/account").WaitForResult();

        protected static IEnumerable<User> FetchUsers()
            => HttpClient.GetAsync<IEnumerable<User>>("users").WaitForResult();
    }

    [Subject("UserService fetch users")]
    public class when_fetching_users : UserModule_specs
    {
        Establish context = () => Initialize();

        Because of = () => Users = FetchUsers();

        It should_not_be_null = () =>
        {
            Users.ShouldNotBeNull();
        };

        It should_not_be_empty = () =>
        {
            Users.ShouldNotBeEmpty();
        };
    }

    [Subject("UserService fetch single user")]
    public class when_fetching_signle_user : UserModule_specs
    {
        Establish context = () => InitializeAndFetchUsers();

        Because of = () => User = FetchUser(UserId);

        It should_not_be_null = () =>
        {
            User.ShouldNotBeNull();
        };

        It should_return_user = () =>
        {
            User.Id.ShouldNotEqual(Guid.Empty);
            User.Name.ShouldNotBeEmpty();
            User.Role.ShouldNotBeEmpty();
            User.State.ShouldNotBeEmpty();
            User.CreatedAt.ShouldNotEqual(DateTime.UtcNow);
        };

        It should_have_correct_id = () =>
        {
            User.UserId.ShouldEqual(UserId);
        };
    }

    [Subject("UserService fetch single user by name")]
    public class when_fetching_single_user_by_name : UserModule_specs
    {
        Establish context = () => InitializeAndFetchUsers();

        Because of = () => User = FetchUserByName(UserName);

        It should_not_be_null = () =>
        {
            User.ShouldNotBeNull();
        };

        It should_return_user = () =>
        {
            User.Id.ShouldNotEqual(Guid.Empty);
            User.Name.ShouldNotBeEmpty();
            User.Role.ShouldNotBeEmpty();
            User.State.ShouldNotBeEmpty();
            User.CreatedAt.ShouldNotEqual(DateTime.UtcNow);
        };

        It should_have_correct_name = () =>
        {
            User.Name.ShouldEqual(UserName);
        };
    }
}