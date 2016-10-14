using System;
using System.Collections.Generic;
using System.Linq;
using Coolector.Dto.Users;
using Coolector.Tests.EndToEnd.Framework;
using Machine.Specifications;

namespace Coolector.Tests.EndToEnd.Services.Storage
{
    public abstract class UserModule_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:10000");
        protected static UserDto User;
        protected static IEnumerable<UserDto> Users;
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

        protected static UserDto FetchUser(string id)
            => HttpClient.GetAsync<UserDto>($"users/{id}").WaitForResult();

        protected static UserDto FetchUserByName(string name)
            => HttpClient.GetAsync<UserDto>($"users/{name}/account").WaitForResult();

        protected static IEnumerable<UserDto> FetchUsers()
            => HttpClient.GetAsync<IEnumerable<UserDto>>("users").WaitForResult();
    }

    [Subject("StorageService fetch users")]
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

    [Subject("StorageService fetch single user")]
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

    [Subject("StorageService fetch single user by name")]
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