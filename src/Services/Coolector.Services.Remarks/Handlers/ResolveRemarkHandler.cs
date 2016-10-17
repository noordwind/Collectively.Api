using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Events.Remarks.Models;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Services;
using RawRabbit;

namespace Coolector.Services.Remarks.Handlers
{
    public class ResolveRemarkHandler : ICommandHandler<ResolveRemark>
    {
        private readonly IBusClient _bus;
        private readonly IRemarkService _remarkService;
        private readonly IFileResolver _fileResolver;
        private readonly IFileValidator _fileValidator;

        public ResolveRemarkHandler(IBusClient bus, 
            IRemarkService remarkService,
            IFileResolver fileResolver,
            IFileValidator fileValidator)
        {
            _bus = bus;
            _remarkService = remarkService;
            _fileResolver = fileResolver;
            _fileValidator = fileValidator;
        }

        public async Task HandleAsync(ResolveRemark command)
        {
            var file = _fileResolver.FromBase64(command.Photo.Base64, command.Photo.Name, command.Photo.ContentType);
            if (file.HasNoValue)
                return;

            var isImage = _fileValidator.IsImage(file.Value);
            if (isImage == false)
                return;

            var location = Location.Create(command.Latitude, command.Longitude);
            await _remarkService.ResolveAsync(command.RemarkId, command.UserId, file.Value, location);

            var remark = await _remarkService.GetAsync(command.RemarkId);

            await _bus.PublishAsync(new RemarkResolved(command.RemarkId, command.UserId, 
                new RemarkFile(remark.Value.ResolvedPhoto.FileId, file.Value.Bytes, remark.Value.ResolvedPhoto.Name,
                    file.Value.ContentType), remark.Value.ResolvedAt.GetValueOrDefault()));
        }
    }
}