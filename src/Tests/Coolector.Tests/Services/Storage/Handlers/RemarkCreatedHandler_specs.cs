using System;
using System.IO;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Events.Remarks.Models;
using Coolector.Dto.Remarks;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Handlers
{
    public abstract class RemarkCreatedHandler_specs
    {
        protected static RemarkCreatedHandler Handler;
        protected static Mock<IFileHandler> FileHandlerMock;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static RemarkCreated Event;
        protected static UserDto User;
        protected static RemarkFile Photo;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            FileHandlerMock = new Mock<IFileHandler>();
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();
            Handler = new RemarkCreatedHandler(FileHandlerMock.Object,
                UserRepositoryMock.Object, RemarkRepositoryMock.Object);
            setup();
        }

        protected static void InitializeUser()
        {
            User = new UserDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid().ToString(),
                Name = "user"
            };
            UserRepositoryMock.Setup(x => x.GetByIdAsync(User.UserId))
                .ReturnsAsync(User);
        }

        protected static void InitializePhoto()
        {
            Photo = new RemarkFile(Guid.NewGuid().ToString(), new byte[] { 0x0 }, "photo.png", "image/png");
        }

        protected static void InitializeEvent()
        {
            Event = new RemarkCreated(Guid.NewGuid(), User?.UserId, new RemarkCreated.RemarkCategory(
                Guid.NewGuid(), "litter"), new RemarkCreated.RemarkLocation(string.Empty, 1, 1), Photo, "test");
        }
    }

    [Subject("RemarkCreatedHandler HandleAsync")]
    public class when_invoking_remark_created_handle_async : RemarkCreatedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializePhoto();
            InitializeEvent();
        });

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_user_repository_get_by_id_async = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(User.UserId), Times.Once);
        };

        It should_call_file_handler_upload_async = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(Photo.Name, Photo.ContentType,
                Moq.It.IsAny<Stream>(), Moq.It.IsAny<Action<string>>()), Times.Once);
        };

        It should_call_remark_repository_add_async = () =>
        {
            RemarkRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<RemarkDto>()), Times.Once);
        };
    }

    [Subject("RemarkCreatedHandler HandleAsync")]
    public class when_invoking_remark_created_handle_async_without_user : RemarkCreatedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializePhoto();
            InitializeEvent();
        });

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Event).Await());

        It should_fail = () => Exception.ShouldBeOfExactType<InvalidOperationException>();

        It should_have_a_specific_reason = () => Exception.Message.ShouldContain("Operation is not valid");
    }

    [Subject("RemarkCreatedHandler HandleAsync")]
    public class when_invoking_remark_created_handle_async_without_photo : RemarkCreatedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            Photo = null;
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Event).Await());

        It should_fail = () => Exception.ShouldBeOfExactType<NullReferenceException>();

        It should_have_a_specific_reason = () => Exception.Message.ShouldContain("Object reference not set");
    }
}