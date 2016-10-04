using System;
using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events.Remarks;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Services;
using RawRabbit;

namespace Coolector.Services.Remarks.Handlers
{
    public class CreaterRemarkHandler : ICommandHandler<CreateRemark>
    {
        private readonly IBusClient _bus;
        private readonly IFileResolver _fileResolver;
        private readonly IRemarkService _remarkService;

        public CreaterRemarkHandler(IBusClient bus, 
            IFileResolver fileResolver, 
            IRemarkService remarkService)
        {
            _bus = bus;
            _fileResolver = fileResolver;
            _remarkService = remarkService;
        }

        public async Task HandleAsync(CreateRemark command)
        {
            var file = _fileResolver.FromBase64(command.Photo.Base64, command.Photo.Name, command.Photo.ContentType);
            if (file.HasNoValue)
                return;

            var remarkId = Guid.NewGuid();
            var position = Position.Create(command.Latitude, command.Longitude);
            await _remarkService.CreateAsync(remarkId, command.UserId, command.CategoryId,
                file.Value, position, command.Description);
            var remark = await _remarkService.GetAsync(remarkId);
            await _bus.PublishAsync(new RemarkCreated(remarkId, command.UserId,
                new RemarkCreated.RemarkCategory(remark.Value.Category.Id, remark.Value.Category.Name),
                new RemarkCreated.RemarkLocation(remark.Value.Location.Address, command.Latitude, command.Longitude),
                new RemarkCreated.RemarkFile(remark.Value.Photo.FileId, file.Value.Bytes, remark.Value.Photo.Name,
                    file.Value.ContentType), command.Description));
        }
    }
}