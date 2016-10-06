using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Providers
{
    public abstract class RemarkProvider_specs
    {
        protected static IRemarkProvider RemarkProvider;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IFileHandler> FileHandlerMock;
        protected static Mock<IProviderClient> ProviderClientMock;
        protected static ProviderSettings ProviderSettings;

        protected static void Initialize()
        {
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            FileHandlerMock = new Mock<IFileHandler>();
            ProviderClientMock = new Mock<IProviderClient>();
            ProviderSettings = new ProviderSettings
            {
                UsersApiUrl = "apiUrl"
            };
            RemarkProvider = new RemarkProvider(RemarkRepositoryMock.Object,
                FileHandlerMock.Object, ProviderClientMock.Object, ProviderSettings);
        }
    }

    [Subject("RemarkProvider BrowseAsync")]
    public class when_invoking_remark_provider_browse_async : RemarkProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => RemarkProvider.BrowseAsync(Moq.It.IsAny<BrowseRemarks>()).Await();

        It should_call_get_collection_using_storage_async = () =>
        {
            ProviderClientMock.Verify(x => x.GetCollectionUsingStorageAsync(
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<Func<Task<Maybe<PagedResult<RemarkDto>>>>>(),
                Moq.It.IsAny<Func<PagedResult<RemarkDto>, Task>>()), Times.Once);
        };
    }

    [Subject("RemarkProvider GetAsync")]
    public class when_invoking_remark_provider_get_async : RemarkProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => RemarkProvider.GetAsync(Moq.It.IsAny<Guid>()).Await();

        It should_call_get_async = () =>
        {
            ProviderClientMock.Verify(x => x.GetUsingStorageAsync(
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<Func<Task<Maybe<RemarkDto>>>>(),
                Moq.It.IsAny<Func<RemarkDto, Task>>()), Times.Once);
        };
    }

    [Subject("RemarkProvider GetPhotoAsync")]
    public class when_invoking_remark_provider_get_photo_async : RemarkProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => RemarkProvider.GetPhotoAsync(Moq.It.IsAny<Guid>()).Await();

        It should_call_file_handler_get_file_stream_info = () =>
        {
            FileHandlerMock.Verify(x => x.GetFileStreamInfoAsync(Moq.It.IsAny<Guid>()), Times.Once);
        };
    }
}