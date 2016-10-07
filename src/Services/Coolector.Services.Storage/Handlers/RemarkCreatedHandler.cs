using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Dto.Common;
using Coolector.Dto.Remarks;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IFileHandler _fileHandler;
        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkCreatedHandler(IFileHandler fileHandler, 
            IUserRepository userRepository, 
            IRemarkRepository remarkRepository)
        {
            _fileHandler = fileHandler;
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            var user = await _userRepository.GetByIdAsync(@event.UserId);
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

            var remark = MapToDto(@event, photo, user.Value);
            await _remarkRepository.AddAsync(remark);
        }

        private static RemarkDto MapToDto(RemarkCreated @event, FileDto photo,
                UserDto user)
            => new RemarkDto
            {
                Id = @event.RemarkId,
                Description = @event.Description,
                Category = new RemarkCategoryDto
                {
                    Id = @event.Category.CategoryId,
                    Name = @event.Category.Name
                },
                Location = new LocationDto
                {
                    Address = @event.Location.Address,
                    Coordinates = new[] {@event.Location.Longitude, @event.Location.Latitude},
                    Type = "Point"
                },
                CreatedAt = DateTime.UtcNow,
                Author = new RemarkAuthorDto
                {
                    UserId = @event.UserId,
                    Name = user.Name
                },
                Photo = photo
            };
    }
}