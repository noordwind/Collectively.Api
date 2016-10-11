using System;
using System.Collections.Generic;
using Coolector.Dto.Users;
using Machine.Specifications;

namespace Coolector.Tests.EndToEnd.API.Modules
{
    public abstract class UserModule_specs : ModuleBase_specs
    {
        protected static void Initialize()
        {
        }

        protected static IEnumerable<UserDto> GetUsers()
            => HttpClient.GetCollectionAsync<UserDto>("users")
                .GetAwaiter()
                .GetResult();
    }

    [Subject("Users collection")]
    public class when_fetching_the_users : UserModule_specs
    {
        static IEnumerable<UserDto> Users;

        Establish context = () => Initialize();

        Because of = () => Users = GetUsers();

        It should_return_non_empty_collection = () =>
        {
            Users.ShouldNotBeEmpty();
            foreach (var user in Users)
            {
                user.Id.ShouldNotEqual(Guid.Empty);
                user.UserId.ShouldNotBeEmpty();
                user.Name.ShouldNotBeEmpty();
                user.Role.ShouldNotBeEmpty();
                user.State.ShouldNotBeEmpty();
                user.CreatedAt.ShouldNotEqual(DateTime.UtcNow);
            }
        };
    }
}