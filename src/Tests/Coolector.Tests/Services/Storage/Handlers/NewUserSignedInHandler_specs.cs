using System;
using Coolector.Common.Events.Users;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Handlers
{
    public abstract class NewUserSignedInHandler_specs
    {
        protected static NewUserSignedInHandler Handler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static NewUserSignedIn Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            Handler = new NewUserSignedInHandler(UserRepositoryMock.Object);
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

        protected static void InitializeEvent()
        {
            Event = new NewUserSignedIn(User?.UserId, User?.Email, User?.Name,
                User?.PictureUrl, User?.Role, User?.CreatedAt ?? DateTime.UtcNow);
        }
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_new_user_signed_in_handle_async : NewUserSignedInHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_user_repository_exists_async = () =>
        {
            UserRepositoryMock.Verify(x => x.ExisitsAsync(User.UserId), Times.Once);
        };

        It should_call_user_repository_add_async = () =>
        {
            UserRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<UserDto>()), Times.Once);
        };
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_new_user_signed_in_for_existing_user : NewUserSignedInHandler_specs
    {
        private Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
            UserRepositoryMock.Setup(x => x.ExisitsAsync(User.UserId)).ReturnsAsync(true);
        });

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_user_repository_exists_async = () =>
        {
            UserRepositoryMock.Verify(x => x.ExisitsAsync(User.UserId), Times.Once);
        };

        It should_not_call_user_repository_add_async = () =>
        {
            UserRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<UserDto>()), Times.Never);
        };
    }
}