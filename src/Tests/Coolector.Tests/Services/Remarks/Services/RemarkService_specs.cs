using System;
using Coolector.Common.Types;
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
        protected static Mock<ICategoryRepository> RemarkCategoryRepositoryMock;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<ICategoryRepository> CategoryRepositoryMock;

        protected static string UserId = "userId";
        protected static User User = new User(UserId, "TestUser");
        protected static File File = File.Create("image.png", "image/png", new byte[] { 1, 2, 3, 4 });
        protected static Guid RemarkId = Guid.NewGuid();
        protected static Location Location = Location.Zero;
        protected static Remark Remark;
        protected static Exception Exception;

        protected static void Initialize()
        {
            FileHandlerMock = new Mock<IFileHandler>();
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            RemarkCategoryRepositoryMock = new Mock<ICategoryRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();
            CategoryRepositoryMock = new Mock<ICategoryRepository>();

            RemarkService = new RemarkService(FileHandlerMock.Object, 
                RemarkRepositoryMock.Object, 
                UserRepositoryMock.Object,
                CategoryRepositoryMock.Object);

            var user = new User(UserId, "name");
            var category = new Category("category");
            var photo = RemarkPhoto.Empty;
            Remark = new Remark(RemarkId, user, category, Location, photo);

            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(Remark);
            UserRepositoryMock.Setup(x => x.GetByUserIdAsync(Moq.It.IsAny<string>()))
                .ReturnsAsync(User);
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

    [Subject("RemarkService ResolveAsync")]
    public class when_resolve_async_is_invoked : RemarkService_specs
    {
        Establish context = () =>
        {
            Initialize();
        };

        Because of = () => RemarkService.ResolveAsync(RemarkId, UserId, File, Location).Await();

        It should_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<Remark>(r => r.Resolved)), Times.Once);
        };

        It should_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(File, Moq.It.IsAny<Action<string>>()), Times.Once);
        };
    }

    [Subject("RemarkService ResolveAsync")]
    public class when_resolve_async_is_invoked_and_user_does_not_exist : RemarkService_specs
    {
        Establish context = () =>
        {
            Initialize();
            UserRepositoryMock.Setup(x => x.GetByUserIdAsync(Moq.It.IsAny<string>()))
                .ReturnsAsync(null);
        };

        Because of = () => 
            Exception = Catch.Exception(() => RemarkService.ResolveAsync(RemarkId, UserId, File, Location).Await());

        It should_throw_argument_exception = () =>
        {
            Exception.ShouldNotBeNull();
            Exception.ShouldBeOfExactType<ArgumentException>();
            Exception.Message.ShouldContain(UserId);
        };

        It should_not_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<Remark>(r => r.Resolved)), Times.Never);
        };

        It should_not_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(File, Moq.It.IsAny<Action<string>>()), Times.Never);
        };
    }

    [Subject("RemarkService ResolveAsync")]
    public class when_resolve_async_is_invoked_and_remark_does_not_exist : RemarkService_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(null);
        };

        Because of = () =>
            Exception = Catch.Exception(() => RemarkService.ResolveAsync(RemarkId, UserId, File, Location).Await());

        It should_throw_argument_exception = () =>
        {
            Exception.ShouldNotBeNull();
            Exception.ShouldBeOfExactType<ServiceException>();
            Exception.Message.ShouldContain(RemarkId.ToString());
        };

        It should_not_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<Remark>(r => r.Resolved)), Times.Never);
        };

        It should_not_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(File, Moq.It.IsAny<Action<string>>()), Times.Never);
        };
    }

    [Subject("RemarkService ResolveAsync")]
    public class when_resolve_async_is_invoked_and_distance_is_too_long : RemarkService_specs
    {
        Establish context = () =>
        {
            Initialize();
            Location = Location.Create(40,40);
        };

        Because of = () =>
            Exception = Catch.Exception(() => RemarkService.ResolveAsync(RemarkId, UserId, File, Location).Await());

        It should_throw_argument_exception = () =>
        {
            Exception.ShouldNotBeNull();
            Exception.ShouldBeOfExactType<ServiceException>();
            Exception.Message.ShouldContain(RemarkId.ToString());
        };

        It should_not_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<Remark>(r => r.Resolved)), Times.Never);
        };

        It should_not_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(File, Moq.It.IsAny<Action<string>>()), Times.Never);
        };
    }
}