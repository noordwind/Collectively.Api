using System;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Commands.Remarks.Models;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Types;
using Coolector.Services.Domain;
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
    public class ResolveRemarkHandler_specs
    {
        protected static ResolveRemarkHandler Handler;
        protected static Mock<IBusClient> BusClientMock;
        protected static Mock<IRemarkService> RemarkServiceMock;
        protected static Mock<IFileResolver> FileResolverMock;
        protected static Mock<IFileValidator> FileValidatorMock;

        protected static ResolveRemark Command;
        protected static string UserId = "UserId";
        protected static Guid RemarkId = Guid.NewGuid();
        protected static File File;
        protected static Remark Remark;
        protected static Location Location;
        protected static User User;
        protected static Category Category;

        protected static Exception Exception;

        protected static void Initialize()
        {
            BusClientMock = new Mock<IBusClient>();
            RemarkServiceMock = new Mock<IRemarkService>();
            FileResolverMock = new Mock<IFileResolver>();
            FileValidatorMock = new Mock<IFileValidator>();

            Handler = new ResolveRemarkHandler(BusClientMock.Object, 
                RemarkServiceMock.Object,
                FileResolverMock.Object,
                FileValidatorMock.Object);

            Command = new ResolveRemark
            {
                RemarkId = RemarkId,
                UserId = UserId,
                Longitude = 1,
                Latitude = 1,
                Photo = new RemarkFile
                {
                    Base64 = "base64",
                    Name = "file.png",
                    ContentType = "image/png"
                }
            };

            File = File.Create(Command.Photo.Name, Command.Photo.ContentType, new byte[] { 0x1 });
            User = new User(UserId, "user");
            Category = new Category("test");
            Location = Location.Create(Command.Latitude, Command.Longitude, "address");
            Remark = new Remark(RemarkId, User, Category, Location,
                RemarkPhoto.Create("file", File.Name, File.Name, File.ContentType), "description");
            Remark.Resolve(User, RemarkPhoto.Create("file-resolved", File.Name, File.Name, File.ContentType));

            FileResolverMock.Setup(x => x.FromBase64(Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(File);
            FileValidatorMock.Setup(x => x.IsImage(Moq.It.IsAny<File>())).Returns(true);
            RemarkServiceMock.Setup(x => x.GetAsync(Moq.It.IsAny<Guid>())).ReturnsAsync(Remark);
        }
    }

    [Subject("ResolveRemarkHandler HandleAsync")]
    public class when_when_invoking_resolve_remark_handle_async : ResolveRemarkHandler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_resolve_file_from_base64 = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name, Command.Photo.ContentType), Times.Once);
        };

        It should_validate_image = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_resolve_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.ResolveAsync(Command.RemarkId, Command.UserId, File, Location), Times.Once);
        };

        It should_fetch_resolved_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Command.RemarkId), Times.Once);
        };

        It should_publish_remark_resolved_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkResolved>(), 
                Moq.It.IsAny<Guid>(), 
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Once);
        };
    }

    [Subject("ResolveRemarkHandler HandleAsync")]
    public class when_when_invoking_resolve_remark_handle_async_and_file_cannot_be_resolved : ResolveRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FileResolverMock.Setup(x => x.FromBase64(
                Moq.It.IsAny<string>(), 
                Moq.It.IsAny<string>(), 
                Moq.It.IsAny<string>()))
                .Returns(new Maybe<File>());
        };

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_resolve_file_from_base64 = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name, Command.Photo.ContentType), Times.Once);
        };

        It should_not_validate_image = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Never);
        };

        It should_not_resolve_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.ResolveAsync(Command.RemarkId, Command.UserId, File, Location), Times.Never);
        };

        It should_not_fetch_resolved_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Command.RemarkId), Times.Never);
        };

        It should_not_publish_remark_resolved_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkResolved>(),
                Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Never);
        };
    }

    [Subject("ResolveRemarkHandler HandleAsync")]
    public class when_when_invoking_resolve_remark_handle_async_and_file_is_not_an_image : ResolveRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FileValidatorMock.Setup(x => x.IsImage(Moq.It.IsAny<File>()))
                .Returns(false);
        };

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_resolve_file_from_base64 = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name, Command.Photo.ContentType), Times.Once);
        };

        It should_validate_image = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_not_resolve_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.ResolveAsync(Command.RemarkId, Command.UserId, File, Location), Times.Never);
        };

        It should_not_fetch_resolved_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Command.RemarkId), Times.Never);
        };

        It should_not_publish_remark_resolved_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkResolved>(),
                Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Never);
        };
    }

    [Subject("ResolveRemarkHandler HandleAsync")]
    public class when_when_invoking_resolve_remark_handle_async_and_latitude_is_corrupt : ResolveRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            Command.Latitude = 100;
        };

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Command).Await());

        It should_throw_argument_exception = () =>
        {
            Exception.ShouldBeOfExactType<ArgumentException>();
            Exception.Message.ShouldContain("Invalid latitude");
        };

        It should_resolve_file_from_base64 = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name, Command.Photo.ContentType), Times.Once);
        };

        It should_validate_image = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_not_resolve_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.ResolveAsync(Command.RemarkId, Command.UserId, File, Location), Times.Never);
        };

        It should_not_fetch_resolved_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Command.RemarkId), Times.Never);
        };

        It should_not_publish_remark_resolved_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkResolved>(),
                Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Never);
        };
    }

    [Subject("ResolveRemarkHandler HandleAsync")]
    public class when_when_invoking_resolve_remark_handle_async_and_longitude_is_corrupt : ResolveRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            Command.Longitude = 200;
        };

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Command).Await());

        It should_throw_argument_exception = () =>
        {
            Exception.ShouldBeOfExactType<ArgumentException>();
            Exception.Message.ShouldContain("Invalid longitude");
        };

        It should_resolve_file_from_base64 = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name, Command.Photo.ContentType), Times.Once);
        };

        It should_validate_image = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_not_resolve_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.ResolveAsync(Command.RemarkId, Command.UserId, File, Location), Times.Never);
        };

        It should_not_fetch_resolved_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Command.RemarkId), Times.Never);
        };

        It should_not_publish_remark_resolved_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkResolved>(),
                Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Never);
        };
    }

    [Subject("ResolveRemarkHandler HandleAsync")]
    public class when_when_invoking_resolve_remark_handle_async_and_resolve_async_fails : ResolveRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkServiceMock.Setup(x => x.ResolveAsync(Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<File>(),
                Moq.It.IsAny<Location>())).Throws<ServiceException>();
        };

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Command).Await());

        It should_throw_service_exception = () =>
        {
            Exception.ShouldBeOfExactType<ServiceException>();
        };

        It should_resolve_file_from_base64 = () =>
        {
            FileResolverMock.Verify(x => x.FromBase64(Command.Photo.Base64, Command.Photo.Name, Command.Photo.ContentType), Times.Once);
        };

        It should_validate_image = () =>
        {
            FileValidatorMock.Verify(x => x.IsImage(File), Times.Once);
        };

        It should_resolve_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.ResolveAsync(Command.RemarkId, Command.UserId, File, Location), Times.Once);
        };

        It should_not_fetch_resolved_remark = () =>
        {
            RemarkServiceMock.Verify(x => x.GetAsync(Command.RemarkId), Times.Never);
        };

        It should_not_publish_remark_resolved_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkResolved>(),
                Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Never);
        };
    }
}