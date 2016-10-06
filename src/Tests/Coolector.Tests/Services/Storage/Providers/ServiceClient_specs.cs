using System;
using System.IO;
using Coolector.Common.Types;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Mappers;
using Coolector.Services.Storage.Providers;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Providers
{
    public abstract class ServiceClient_specs
    {
        protected static IServiceClient ServiceClient;
        protected static Mock<IHttpClient> HttpClientMock;
        protected static Mock<IMapperResolver> MapperResolverMock;
        protected static string Url = "http://test";
        protected static string Endpoint = "users";

        protected static void Initialize()
        {
            HttpClientMock = new Mock<IHttpClient>();
            MapperResolverMock = new Mock<IMapperResolver>();
            ServiceClient = new ServiceClient(HttpClientMock.Object, MapperResolverMock.Object);
        }
    }

    [Subject("ServiceClient GetAsync")]
    public class when_invoking_service_client_get_async : ServiceClient_specs
    {
        static AwaitResult<Maybe<UserDto>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetAsync<UserDto>(Url, Endpoint).Await();

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync(Url, Endpoint), Times.Once);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }

    [Subject("ServiceClient GetCollectionAsync")]
    public class when_invoking_service_client_get_collection_async : ServiceClient_specs
    {
        static AwaitResult<Maybe<PagedResult<UserDto>>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetCollectionAsync<UserDto>(Url, Endpoint).Await();

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync(Url, Endpoint), Times.Once);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }

    [Subject("ServiceClient GetCollectionAsync")]
    public class when_invoking_service_client_get_stream_async : ServiceClient_specs
    {
        static AwaitResult<Maybe<Stream>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetStreamAsync(Url, Endpoint).Await();

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync(Url, Endpoint), Times.Once);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }

    [Subject("ServiceClient GetAsync")]
    public class when_invoking_service_client_get_async_with_invalid_url : ServiceClient_specs
    {
        static AwaitResult<Maybe<UserDto>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetAsync<UserDto>(Url, Endpoint).Await();

        It should_not_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync("test", Endpoint), Times.Never);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }
}