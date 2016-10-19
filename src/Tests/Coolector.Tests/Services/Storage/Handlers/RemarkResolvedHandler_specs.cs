using System;
using System.IO;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Events.Remarks.Models;
using Coolector.Common.Types;
using Coolector.Dto.Common;
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
    public class RemarkResolvedHandler_specs
    {
        protected static RemarkResolvedHandler Handler;
        protected static Mock<IFileHandler> FileHandlerMock;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IUserRepository> UserRepositoryMock;

        protected static RemarkResolved Event;
        protected static Guid RemarkId = Guid.NewGuid();
        protected static string UserId = "UserId";
        protected static RemarkFile Photo;
        protected static DateTime CreatedAt = DateTime.Now - TimeSpan.FromMinutes(5.0);
        protected static DateTime ResolvedAt = DateTime.Now;
        protected static RemarkDto Remark;
        protected static RemarkAuthorDto Author;
        protected static RemarkCategoryDto Category;
        protected static LocationDto Location;
        protected static FileDto File;
        protected static UserDto User;

        protected static void Initialize()
        {
            FileHandlerMock = new Mock<IFileHandler>();
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();

            Handler = new RemarkResolvedHandler(FileHandlerMock.Object,
                RemarkRepositoryMock.Object,
                UserRepositoryMock.Object);

            Photo = new RemarkFile("internalId", new byte[] {1,2,3}, "image.png", "image/png" );
            Event = new RemarkResolved(RemarkId, UserId, Photo, ResolvedAt);
            Author = new RemarkAuthorDto
            {
                UserId = UserId,
                Name = "TestUser"
            };
            Category = new RemarkCategoryDto
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            Location = new LocationDto
            {
                Address = "address",
                Coordinates = new[] {1.0, 1.0},
                Type = "point"
            };
            File = new FileDto
            {
                Name = "image.png",
                ContentType = "image/png",
                FileId = "fileId"
            };
            Remark = new RemarkDto
            {
                Id = RemarkId,
                Author = Author,
                Category = Category,
                CreatedAt = CreatedAt,
                Description = "test",
                Location = Location,
                Photo = File
            };
            User = new UserDto
            {
                UserId = UserId,
                Name = "TestUser"
            };

            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(Remark);
            UserRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<string>()))
                .ReturnsAsync(User);
        }
    }

    [Subject("RemarkResolvedHandler HandleAsync")]
    public class when_invoking_handle_async : RemarkResolvedHandler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_fetch_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.GetByIdAsync(RemarkId), Times.Once);
        };

        It should_fetch_user = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(UserId), Times.Once);
        };

        It should_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(
                Event.Photo.Name, 
                Event.Photo.ContentType,
                Moq.It.IsAny<MemoryStream>(), 
                Moq.It.IsAny<Action<string>>()), Times.Once);
        };

        It should_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<RemarkDto>(r => r.Resolved
                && r.ResolvedAt == Event.ResolvedAt
                && r.Resolver.UserId == Event.UserId
                && r.ResolvedPhoto.Name == Event.Photo.Name)));
        };
    }

    [Subject("RemarkResolvedHandler HandleAsync")]
    public class when_invoking_handle_async_and_remark_does_not_exist : RemarkResolvedHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(null);
        };

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_fetch_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.GetByIdAsync(RemarkId), Times.Once);
        };

        It should_not_fetch_user = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(UserId), Times.Never);
        };

        It should_not_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(
                Event.Photo.Name, 
                Event.Photo.ContentType,
                Moq.It.IsAny<MemoryStream>(), 
                Moq.It.IsAny<Action<string>>()), Times.Never);
        };

        It should_not_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<RemarkDto>(r => r.Resolved
                && r.ResolvedAt == Event.ResolvedAt
                && r.Resolver.UserId == Event.UserId
                && r.ResolvedPhoto.Name == Event.Photo.Name)), Times.Never);
        };
    }

    [Subject("RemarkResolvedHandler HandleAsync")]
    public class when_invoking_handle_async_and_user_does_not_exist : RemarkResolvedHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            UserRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<string>()))
                .ReturnsAsync(null);
        };

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_fetch_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.GetByIdAsync(RemarkId), Times.Once);
        };

        It should_fetch_user = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(UserId), Times.Once);
        };

        It should_not_upload_file = () =>
        {
            FileHandlerMock.Verify(x => x.UploadAsync(
                Event.Photo.Name, 
                Event.Photo.ContentType,
                Moq.It.IsAny<MemoryStream>(), 
                Moq.It.IsAny<Action<string>>()), Times.Never);
        };

        It should_not_update_remark = () =>
        {
            RemarkRepositoryMock.Verify(x => x.UpdateAsync(Moq.It.Is<RemarkDto>(r => r.Resolved
                && r.ResolvedAt == Event.ResolvedAt
                && r.Resolver.UserId == Event.UserId
                && r.ResolvedPhoto.Name == Event.Photo.Name)), Times.Never);
        };
    }
}