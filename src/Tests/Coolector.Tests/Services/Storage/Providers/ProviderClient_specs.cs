using Coolector.Common.Types;
using Coolector.Services.Storage.Providers;
using Coolector.Tests.Framework;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Providers
{
    public abstract class ProviderClient_specs
    {
        protected static IProviderClient ProviderClient;
        protected static Mock<IServiceClient> ServiceClientMock;
        protected static Mock<IStorage> StorageMock;

        protected static void Initialize()
        {
            ServiceClientMock = new Mock<IServiceClient>();
            StorageMock = new Mock<IStorage>();

            ProviderClient = new ProviderClient(ServiceClientMock.Object);
        }
    }

    [Subject("ProviderClient GetAsync")]
    public class When_get_async : ProviderClient_specs
    {
        Establish context = () => Initialize();

        Because of = () => ProviderClient.GetAsync<object>("url", "endpoint");

        It should_call_service_client_get_async = () =>
        {
            ServiceClientMock.Verify(x => x.GetAsync<object>(
                Moq.It.IsAny<string>(), 
                Moq.It.IsAny<string>()), Times.Once);
        };
    }

    [Subject("ProviderClient GetSreamAsync")]
    public class When_get_stream_async : ProviderClient_specs
    {
        Establish context = () => Initialize();

        Because of = () => ProviderClient.GetStreamAsync("url", "endpoint");

        It should_call_service_client_get_stream_async = () =>
        {
            ServiceClientMock.Verify(x => x.GetStreamAsync(
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>()), Times.Once);
        };
    }

    [Subject("ProviderClient GetUsingStorageAsync")]
    public class When_get_using_storage_async_and_storage_has_data : ProviderClient_specs
    {
        protected static AwaitResult<Maybe<object>> Result;

        Establish context = () =>
        {
            Initialize();
            StorageMock.Setup(x => x.FetchAsync()).ReturnsAsync(new object());
        };

        Because of = () => Result = ProviderClient.GetUsingStorageAsync("url", "endpoint",
            async () => await StorageMock.Object.FetchAsync(),
            async obj => await StorageMock.Object.SaveAsync(obj)).Await();

        It should_call_fetch_data = () =>
        {
            StorageMock.Verify(x => x.FetchAsync(), Times.Once);
        };

        It should_not_call_save_data = () =>
        {
            StorageMock.Verify(x => x.SaveAsync(Moq.It.IsAny<object>()), Times.Never);
        };

        It should_return_a_value = () =>
        {
            Result.AsTask.Result.HasValue.ShouldBeTrue();
        };
    }

    [Subject("ProviderClient GetUsingStorageAsync")]
    public class When_get_using_storage_async_and_storage_is_empty : ProviderClient_specs
    {
        protected static AwaitResult<Maybe<object>> Result;
        
        Establish context = () =>
        {
            Initialize();
            StorageMock.Setup(x => x.FetchAsync()).ReturnsAsync(null);
            ServiceClientMock.Setup(x => x.GetAsync<object>(
                Moq.It.IsAny<string>(), 
                Moq.It.IsAny<string>()))
                .ReturnsAsync(new object());
        };

        Because of = () => Result = ProviderClient.GetUsingStorageAsync("url", "endpoint",
            async () => await StorageMock.Object.FetchAsync(),
            async obj => await StorageMock.Object.SaveAsync(obj)).Await();

        It should_call_fetch_data = () =>
        {
            StorageMock.Verify(x => x.FetchAsync(), Times.Once);
        };

        It should_call_save_data = () =>
        {
            StorageMock.Verify(x => x.SaveAsync(Moq.It.IsAny<object>()), Times.Once);
        };

        It should_return_a_value = () =>
        {
            Result.AsTask.Result.HasValue.ShouldBeTrue();
        };
    }

    public class When_get_using_storage_async_and_data_does_not_exist : ProviderClient_specs
    {
        protected static AwaitResult<Maybe<object>> Result;

        Establish context = () =>
        {
            Initialize();
            StorageMock.Setup(x => x.FetchAsync()).ReturnsAsync(null);
            ServiceClientMock.Setup(x => x.GetAsync<object>(
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>()))
                .ReturnsAsync(null);
        };

        Because of = () => Result = ProviderClient.GetUsingStorageAsync("url", "endpoint",
            async () => await StorageMock.Object.FetchAsync(),
            async obj => await StorageMock.Object.SaveAsync(obj)).Await();

        It should_call_fetch_data = () =>
        {
            StorageMock.Verify(x => x.FetchAsync(), Times.Once);
        };

        It should_call_save_data = () =>
        {
            StorageMock.Verify(x => x.SaveAsync(Moq.It.IsAny<object>()), Times.Never);
        };

        It should_return_structure_without_value = () =>
        {
            Result.AsTask.Result.HasNoValue.ShouldBeTrue();
        };
    }
}