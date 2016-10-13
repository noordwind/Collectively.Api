using System;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events.Remarks;
using Coolector.Services.Domain;
using Coolector.Services.Remarks.Handlers;
using Coolector.Services.Remarks.Services;
using Machine.Specifications;
using Moq;
using RawRabbit;
using RawRabbit.Configuration.Publish;
using It = Machine.Specifications.It;

namespace Coolector.Tests.Services.Remarks.Handlers
{
    public abstract class DeleteRemarkHandler_specs
    {
        protected static DeleteRemarkHandler Handler;
        protected static Mock<IBusClient> BusClientMock;
        protected static Mock<IRemarkService> RemarkServiceMock;
        protected static DeleteRemark Command;
        protected static Exception Exception;

        protected static void Initialize()
        {
            BusClientMock = new Mock<IBusClient>();
            RemarkServiceMock = new Mock<IRemarkService>();
            Command = new DeleteRemark
            {
                UserId = "userId",
                RemarkId = Guid.NewGuid()
            };
            Handler = new DeleteRemarkHandler(BusClientMock.Object, RemarkServiceMock.Object);
        }
    }

    [Subject("DeleteRemarkHandler HandleAsync")]
    public class when_invoking_delete_remark_handle_async : DeleteRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
        };

        Because of = () => Handler.HandleAsync(Command).Await();

        It should_call_delete_async_on_remark_service = () =>
        {
            RemarkServiceMock.Verify(x => x.DeleteAsync(Command.RemarkId, Command.UserId), Times.Once);
        };

        It should_publish_remark_deleted_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkDeleted>(), 
                Moq.It.IsAny<Guid>(), 
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Once);
        };
    }


    [Subject("DeleteRemarkHandler HandleAsync")]
    public class when_invoking_delete_remark_handle_async_and_service_throws_exception : DeleteRemarkHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkServiceMock.Setup(x => x.DeleteAsync(Command.RemarkId, Command.UserId))
                .Throws<ServiceException>();
        };

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Command).Await());

        It should_call_delete_async_on_remark_service = () =>
        {
            RemarkServiceMock.Verify(x => x.DeleteAsync(Command.RemarkId, Command.UserId), Times.Once);
        };

        It should_not_publish_remark_deleted_event = () =>
        {
            BusClientMock.Verify(x => x.PublishAsync(Moq.It.IsAny<RemarkDeleted>(),
                Moq.It.IsAny<Guid>(),
                Moq.It.IsAny<Action<IPublishConfigurationBuilder>>()), Times.Never);
        };
    }
}