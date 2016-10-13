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
    public class CreateRemarkHandler : ICommandHandler<CreateRemark>
    {
        private readonly IBusClient _bus;
        private readonly IFileResolver _fileResolver;
        private readonly IFileValidator _fileValidator;
        private readonly IRemarkService _remarkService;

        public CreateRemarkHandler(IBusClient bus, 
            IFileResolver fileResolver, 
            IFileValidator fileValidator,
            IRemarkService remarkService)
        {
            _bus = bus;
            _fileResolver = fileResolver;
            _fileValidator = fileValidator;
            _remarkService = remarkService;
        }

        public async Task HandleAsync(CreateRemark command)
        {
            var file = _fileResolver.FromBase64(command.Photo.Base64, command.Photo.Name, command.Photo.ContentType);
            if (file.HasNoValue)
                return;

            var isImage = _fileValidator.IsImage(file.Value);
            if(!isImage)
                return;

            var remarkId = Guid.NewGuid();
            var location = Location.Create(command.Latitude, command.Longitude, command.Address);
            await _remarkService.CreateAsync(remarkId, command.UserId, command.CategoryId,
                file.Value, location, command.Description);
            var remark = await _remarkService.GetAsync(remarkId);
            await _bus.PublishAsync(new RemarkCreated(remarkId, command.UserId,
                new RemarkCreated.RemarkCategory(remark.Value.Category.Id, remark.Value.Category.Name),
                new RemarkCreated.RemarkLocation(remark.Value.Location.Address, command.Latitude, command.Longitude),
                new RemarkCreated.RemarkFile(remark.Value.Photo.FileId, file.Value.Bytes, remark.Value.Photo.Name,
                    file.Value.ContentType), command.Description));
        }
    }
}