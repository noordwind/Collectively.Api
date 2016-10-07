using System;
using Coolector.Services.Domain;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Repositories;
using Coolector.Services.Remarks.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Remarks.Services
{
    public abstract class RemarkService_specs
    {
        protected static IRemarkService RemarkService;
        protected static Mock<IFileHandler> FileHandlerMock;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<ICategoryRepository> CategoryRepositoryMock;

        protected static string UserId = "userId";
        protected static Guid RemarkId = Guid.NewGuid();
        protected static Remark Remark;
        protected static Exception Exception;

        protected static void Initialize()
        {
            FileHandlerMock = new Mock<IFileHandler>();
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();
            CategoryRepositoryMock = new Mock<ICategoryRepository>();

            RemarkService = new RemarkService(FileHandlerMock.Object, 
                RemarkRepositoryMock.Object, UserRepositoryMock.Object,
                CategoryRepositoryMock.Object);

            var user = new User(UserId, "name");
            var category = new Category("category");
            var location = Location.Zero;
            var photo = RemarkPhoto.Empty;
            Remark = new Remark(RemarkId, user, category, location, photo);

            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(Remark);
        }
    }

    [Subject("RemarkService DeleteAsync")]
    public class when_delete_async_is_invoked : RemarkService_specs
    {
        Establish context = () => Initialize();

        Because of = () => RemarkService.DeleteAsync(RemarkId, UserId).Await();

        It should_call_delete_async_on_file_handler = () =>
        {
            FileHandlerMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<string>()), Times.Once);
        };

        It should_call_delete_async_on_remark_repository = () =>
        {
            RemarkRepositoryMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<Remark>()), Times.Once);
        };
    }

    [Subject("RemarkService DeleteAsync")]
    public class when_delete_async_is_invoked_but_remark_doesnt_exist : RemarkService_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(null);
        };

        Because of = () => Exception = Catch.Exception(() => RemarkService.DeleteAsync(RemarkId, UserId).Await());

        It should_throw_service_exception = () =>
        {
            Exception.ShouldBeOfExactType<ServiceException>();
        };

        It should_not_call_delete_async_on_file_handler = () =>
        {
            FileHandlerMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<string>()), Times.Never);
        };

        It should_not_call_delete_async_on_remark_repository = () =>
        {
            RemarkRepositoryMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<Remark>()), Times.Never);
        };
    }


    [Subject("RemarkService DeleteAsync")]
    public class when_delete_async_is_invoked_but_user_is_not_author : RemarkService_specs
    {
        Establish context = () =>
        {
            Initialize();
        };

        Because of = () => Exception = Catch.Exception(() => RemarkService.DeleteAsync(RemarkId, "not_author").Await());

        It should_throw_service_exception = () =>
        {
            Exception.ShouldBeOfExactType<ServiceException>();
        };

        It should_not_call_delete_async_on_file_handler = () =>
        {
            FileHandlerMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<string>()), Times.Never);
        };

        It should_not_call_delete_async_on_remark_repository = () =>
        {
            RemarkRepositoryMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<Remark>()), Times.Never);
        };
    }
}