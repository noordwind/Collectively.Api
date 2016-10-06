using System;
using Coolector.Common.Events.Users;
using Coolector.Dto.Users;
using Coolector.Services.Domain;
using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Storage.Handlers
{
    public abstract class UsernameChangedHandler_specs
    {
        protected static UserNameChangedHandler Handler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static UserNameChanged Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            Handler = new UserNameChangedHandler(UserRepositoryMock.Object);
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
            Event = new UserNameChanged(User?.UserId, User?.Name);
        }
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_username_changed_handle_async : UsernameChangedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_user_repository_get_by_id_async = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(User.UserId), Times.Once);
        };

        It should_call_user_repository_edit_async = () =>
        {
            UserRepositoryMock.Verify(x => x.EditAsync(Moq.It.IsAny<UserDto>()), Times.Once);
        };
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_username_changed_handle_async_without_user : UsernameChangedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeEvent();
        });

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Event).Await());

        It should_fail = () => Exception.ShouldBeOfExactType<ServiceException>();

        It should_have_a_specific_reason = () => Exception.Message.ShouldContain("User name cannot be changed because");
    }
}