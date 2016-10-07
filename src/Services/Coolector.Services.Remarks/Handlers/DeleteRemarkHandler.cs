using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events.Remarks;
using Coolector.Services.Remarks.Services;
using RawRabbit;

namespace Coolector.Services.Remarks.Handlers
{
    public class DeleteRemarkHandler : ICommandHandler<DeleteRemark>
    {
        private readonly IBusClient _bus;
        private readonly IRemarkService _remarkService;

        public DeleteRemarkHandler(IBusClient bus, IRemarkService remarkService)
        {
            _bus = bus;
            _remarkService = remarkService;
        }

        public async Task HandleAsync(DeleteRemark command)
        {
            await _remarkService.DeleteAsync(command.RemarkId, command.UserId);
            await _bus.PublishAsync(new RemarkDeleted(command.RemarkId));
        }
    }
}