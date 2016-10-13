using System;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Handlers;
using Coolector.Services.Remarks.Services;
using Machine.Specifications;
using Moq;
using RawRabbit;
using RawRabbit.Configuration.Publish;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Remarks.Handlers
{
    public abstract class CreateRemarkHandler_specs
    {
        protected static CreateRemarkHandler Handler;
        protected static Mock<IBusClient> BusClientMock;
        protected static Mock<IFileResolver> FileResolverMock;
        protected static Mock<IFileValidator> FileValidatorMock;
        protected static Mock<IRemarkService> RemarkServiceMock;
        protected static CreateRemark Command;
        protected static Exception Exception;

        protected static void Initialize()
        {
            BusClientMock = new Mock<IBusClient>();
            FileResolverMock = new Mock<IFileResolver>();
            FileValidatorMock = new Mock<IFileValidator>();
            RemarkServiceMock = new Mock<IRemarkService>();
            Command = new CreateRemark
            {
                UserId = "userId",
                CategoryId = Guid.NewGuid(),
                Longitude = 1,
                Latitude = 1,
                Description = "test",
                Address = "address",
                Photo = new CreateRemark.RemarkFile
                {
                    Base64 = "base64",
                    Name = "file.png",
                    ContentType = "image/png"
                }
            };
            Handler = new CreateRemarkHandler(BusClientMock.Object, FileResolverMock.Object,
                FileValidatorMock.Object, RemarkServiceMock.Object);
        }
    }

    [Subject("CreateRemarkHandler HandleAsync")]
    public class when_invoking_create_remark_handle_async : CreateRemarkHandler_specs
    {
        static Location Location;
        static File File;
        static Remark Remark;

        Establish context = () =>
        {
            Initialize();
            Location = Location.Create(Command.Latitude, Command.Longitude, Command.Address);
            File = File.Create(Command.Photo.Name, Command.Photo.ContentType, new byte[] { 0x1 });
            Remark = new Remark(Guid.NewGuid(), new User(Command.UserId, "user"), new Category("test"), Location, 
                RemarkPhoto.Create("file", File.Name, File.Name, File.ContentType), Command.Description);
            FileResolverMock.Setup(x => x.FromBase64(Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(File);
            FileValidatorMock.Setup(x => x.IsImage(Moq.It.IsAny<File>())).Returns(true);
            RemarkServiceMock.Setup(x => x.GetAsync(Moq.It.IsAny<Guid>())).ReturnsAsync(Remark);
        };

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_call_from_base_64_on_file_resolver = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name,
                Command.Photo.ContentType), Times.Once);
        };

        It should_call_is_image_on_file_validator = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_call_create_async_on_remark_service = () =>
        {
            RemarkServiceMock.Verify(x => x.CreateAsync(Moq.It.IsAny<Guid>(), Command.UserId,
                Command.CategoryId, File, Location, Command.Description), Times.Once);
        };

        It should_call_get_async_on_remark_service = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Moq.It.IsAny<Guid>()), Times.Once);
        };

        It should_publish_remark_created_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkCreated>(), 
                Moq.It.IsAny<Guid>(), 
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Once);
        };
    }

    [Subject("CreateRemarkHandler HandleAsync")]
    public class when_invoking_create_remark_handle_async_without_file : CreateRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FileResolverMock.Setup(x => x.FromBase64(Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(new Maybe<File>());
        };

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_call_from_base_64_on_file_resolver = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name,
                Command.Photo.ContentType), Times.Once);
        };


        It should_not_call_is_image_on_file_validator = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(Moq.It.IsAny<File>()), Times.Never);
        };
    }

    [Subject("CreateRemarkHandler HandleAsync")]
    public class when_invoking_create_remark_handle_async_with_invalid_file : CreateRemarkHandler_specs
    {
        static File File;

        Establish context = () =>
        {
            Initialize();
            File = File.Create(Command.Photo.Name, Command.Photo.ContentType, new byte[] { 0x1 });
            FileResolverMock.Setup(x => x.FromBase64(Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(File);
            FileValidatorMock.Setup(x => x.IsImage(Moq.It.IsAny<File>())).Returns(false);

        };

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_call_from_base_64_on_file_resolver = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name,
                Command.Photo.ContentType), Times.Once);
        };

        It should_call_is_image_on_file_validator = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_not_call_create_async_on_remark_service = () =>
        {
            RemarkServiceMock.Verify(x => x.CreateAsync(Moq.It.IsAny<Guid>(), Command.UserId,
                Command.CategoryId, File, Moq.It.IsAny<Location>(), Command.Description), Times.Never);
        };
    }
}