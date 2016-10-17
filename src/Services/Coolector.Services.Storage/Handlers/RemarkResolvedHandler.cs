using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Dto.Common;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IFileHandler _fileHandler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IUserRepository _userRepository;

        public RemarkResolvedHandler(IFileHandler fileHandler, 
            IRemarkRepository remarkRepository,
            IUserRepository userRepository)
        {
            _fileHandler = fileHandler;
            _remarkRepository = remarkRepository;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
            if (remark.HasNoValue)
                return;

            var user = await _userRepository.GetByIdAsync(@event.UserId);
            if (user.HasNoValue)
                return;

            var photo = new FileDto
            {
                Name = @event.Photo.Name,
                ContentType = @event.Photo.ContentType,
            };

            using (var memoryStream = new MemoryStream(@event.Photo.Bytes))
            {
                await _fileHandler.UploadAsync(photo.Name, photo.ContentType,
                    memoryStream, fileId =>
                    {
                        photo.FileId = fileId;
                    });
            }

            remark.Value.Resolved = true;
            remark.Value.ResolvedAt = @event.ResolvedAt;
            remark.Value.Resolver = new RemarkAuthorDto
            {
                UserId = user.Value.UserId,
                Name = user.Value.Name
            };
            remark.Value.ResolvedPhoto = photo;
            await _remarkRepository.UpdateAsync(remark.Value);
        }
    }
}