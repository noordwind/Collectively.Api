using System;
using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Remarks;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Services;

namespace Coolector.Services.Remarks.Handlers
{
    public class CreaterRemarkHandler : ICommandHandler<CreateRemark>
    {
        private readonly IFileResolver _fileResolver;
        private readonly IRemarkService _remarkService;

        public CreaterRemarkHandler(IFileResolver fileResolver, IRemarkService remarkService)
        {
            _fileResolver = fileResolver;
            _remarkService = remarkService;
        }

        public async Task HandleAsync(CreateRemark command)
        {
            var file = _fileResolver.FromBase64(command.Photo.Base64, command.Photo.Name, command.Photo.ContentType);
            if(file.HasNoValue)
                return;

            var position = Position.Create(command.Latitude, command.Longitude);
            await _remarkService.CreateAsync(command.UserId, command.CategoryId, file.Value, position, command.Description);
        }
    }
}