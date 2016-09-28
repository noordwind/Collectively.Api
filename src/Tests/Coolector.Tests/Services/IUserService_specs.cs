//using System;
//using Coolector.Common.Events.Users;
//using Coolector.Core.Domain;
//using Coolector.Core.Events;
//using Machine.Specifications;
//using Moq;
//using It = Machine.Specifications.It;

//namespace Coolector.Tests.Services
//{
//    public abstract class IUserService_specs
//    {
//        protected static IUserService UserService;
//        protected static Mock<IUserRepository> UserRepositoryMock;
//        protected static Mock<IEventDispatcher> EventDispatcherMock;
//        protected static User TestUser;
//        protected static Exception Exception;

//        protected static void Initialize()
//        {
//            UserRepositoryMock = new Mock<IUserRepository>();
//            EventDispatcherMock = new Mock<IEventDispatcher>();

//            TestUser = new User("email", externalId: "externalId");

//            UserService = new UserService(UserRepositoryMock.Object, EventDispatcherMock.Object);
//        }

//        protected static void SetupMocks()
//        {
//            UserRepositoryMock.Setup(x => x.GetByEmailAsync(Moq.It.IsAny<string>())).ReturnsAsync(TestUser);
//        }
//    }

//    [Subject("UserService")]
//    public class when_user_wants_to_sign_in : IUserService_specs
//    {
//        Establish context => () =>
//        {
//            Initialize();
//            SetupMocks();
//        };

//        Because of => () => UserService.SignInUserAsync("email", "externalId", "picture");

//        It should_call_get_user_by_email => () =>
//        {
//            UserRepositoryMock.Verify(x => x.GetByEmailAsync(Moq.It.IsAny<string>()), Times.Once);
//        };

//        It should_publish_user_signed_in_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<UserSignedIn>()), Times.Once);
//        };

//        It should_not_publish_new_user_signed_up_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<NewUserSignedUp>()), Times.Never);
//        };
//    }

//    [Subject("UserService")]
//    public class when_user_wants_to_sign_up : IUserService_specs
//    {
//        Establish context => Initialize;

//        Because of => () => UserService.SignInUserAsync("email", "externalId", "picture");

//        It should_call_get_user_by_email => () =>
//        {
//            UserRepositoryMock.Verify(x => x.GetByEmailAsync(Moq.It.IsAny<string>()), Times.Once);
//        };

//        It should_publish_new_user_signed_up_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<NewUserSignedUp>()), Times.Once);
//        };

//        It should_not_publish_user_signed_in_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<UserSignedIn>()), Times.Never);
//        };
//    }

//    [Subject("UserService")]
//    public class when_user_wants_to_sign_in_but_email_is_empty : IUserService_specs
//    {
//        Establish context => Initialize;

//        Because of
//            => () => Exception = Catch.Exception(() => UserService.SignInUserAsync("", "externalId", "picture"));

//        It should_throw_service_exception => () =>
//        {
//            Exception.ShouldBeOfExactType<ServiceException>();
//            Exception.Message.ShouldEqual("Email cannot be empty");
//        };

//        It should_not_publish_new_user_signed_up_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<NewUserSignedUp>()), Times.Never);
//        };

//        It should_not_publish_user_signed_in_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<UserSignedIn>()), Times.Never);
//        };
//    }

//    [Subject("UserService")]
//    public class when_user_wants_to_sign_in_but_external_id_is_empty : IUserService_specs
//    {
//        Establish context => Initialize;

//        Because of
//            => () => Exception = Catch.Exception(() => UserService.SignInUserAsync("email", "", "picture"));

//        It should_throw_service_exception => () =>
//        {
//            Exception.ShouldBeOfExactType<ServiceException>();
//            Exception.Message.ShouldEqual("ExternalId cannot be empty");
//        };

//        It should_not_publish_new_user_signed_up_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<NewUserSignedUp>()), Times.Never);
//        };

//        It should_not_publish_user_signed_in_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<UserSignedIn>()), Times.Never);
//        };
//    }

//    [Subject("UserService")]
//    public class when_user_wants_to_sign_in_but_email_is_null : IUserService_specs
//    {
//        Establish context => Initialize;

//        Because of
//            => () => Exception = Catch.Exception(() => UserService.SignInUserAsync(null, "externalId", "picture"));

//        It should_throw_service_exception => () =>
//        {
//            Exception.ShouldBeOfExactType<ServiceException>();
//            Exception.Message.ShouldEqual("Email cannot be empty");
//        };

//        It should_not_publish_new_user_signed_up_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<NewUserSignedUp>()), Times.Never);
//        };

//        It should_not_publish_user_signed_in_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<UserSignedIn>()), Times.Never);
//        };
//    }

//    [Subject("UserService")]
//    public class when_user_wants_to_sign_in_but_external_id_is_null : IUserService_specs
//    {
//        Establish context => Initialize;

//        Because of
//            => () => Exception = Catch.Exception(() => UserService.SignInUserAsync("email", null, "picture"));

//        It should_throw_service_exception => () =>
//        {
//            Exception.ShouldBeOfExactType<ServiceException>();
//            Exception.Message.ShouldEqual("ExternalId cannot be empty");
//        };

//        It should_not_publish_new_user_signed_up_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<NewUserSignedUp>()), Times.Never);
//        };

//        It should_not_publish_user_signed_in_event => () =>
//        {
//            EventDispatcherMock.Verify(x => x.DispatchAsync(Moq.It.IsAny<UserSignedIn>()), Times.Never);
//        };
//    }
//}