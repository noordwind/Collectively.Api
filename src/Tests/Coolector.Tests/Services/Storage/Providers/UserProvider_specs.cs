using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Providers
{
    public abstract class UserProvider_specs
    {
        protected static IUserProvider UserProvider;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<IProviderClient> ProviderClientMock;
        protected static ProviderSettings ProviderSettings;

        protected static void Initialize()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            ProviderClientMock = new Mock<IProviderClient>();
            ProviderSettings = new ProviderSettings
            {
                UsersApiUrl = "apiUrl"
            };

            UserProvider = new UserProvider(UserRepositoryMock.Object,
                ProviderClientMock.Object, ProviderSettings);
        }
    }

    [Subject("UserProvider BrowseAsync")]
    public class When_browse_users : UserProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => UserProvider.BrowseAsync(new BrowseUsers());

        It should_call_get_collection_using_storage = () =>
        {
            ProviderClientMock.Verify(x => x.GetCollectionUsingStorageAsync(
                Moq.It.IsAny<string>(), 
                Moq.It.IsAny<string>(), 
                Moq.It.IsAny<Func<Task<Maybe<PagedResult<UserDto>>>>>(),
                Moq.It.IsAny<Func<PagedResult<UserDto>, Task>>()), Times.Once);
        };
    }

    [Subject("UserProvider GetAsync")]
    public class When_get_user : UserProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => UserProvider.GetAsync("userId");

        It should_call_get_using_storage = () =>
        {
            ProviderClientMock.Verify(x => x.GetUsingStorageAsync(
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<Func<Task<Maybe<UserDto>>>>(),
                Moq.It.IsAny<Func<UserDto, Task>>()), Times.Once);
        };
    }
}