using System;
using Coolector.Common.Events.Users;
using Coolector.Services.Remarks.Handlers;
using Coolector.Services.Remarks.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Remarks.Handlers
{
    public abstract class NewUserSignedInHandler_specs
    {
        protected static NewUserSignedInHandler Handler;
        protected static Mock<IUserService> UserServiceMock;
        protected static NewUserSignedIn Event;
        protected static Exception Exception;

        protected static void Initialize()
        {
            UserServiceMock = new Mock<IUserService>();
            Event = new NewUserSignedIn("user", "user@email.com", "name",
                "picture", "user", "active", DateTime.UtcNow);
            Handler = new NewUserSignedInHandler(UserServiceMock.Object);
        }
    }

    [Subject("NewUserSignedInHandler HandleAsync")]
    public class when_invoking_new_user_signed_in_handle_async : NewUserSignedInHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
        };

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_create_if_not_found_async_on_user_service = () =>
        {
            UserServiceMock.Verify(x => x.CreateIfNotFoundAsync(Event.UserId, Event.Name), Times.Once);
        };
    }
}