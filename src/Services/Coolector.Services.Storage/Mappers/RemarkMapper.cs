using Coolector.Dto.Common;
using Coolector.Dto.Remarks;

namespace Coolector.Services.Storage.Mappers
{
    public class RemarkMapper : IMapper<RemarkDto>
    {
        public RemarkDto Map(dynamic source)
        {
            return new RemarkDto
            {
                Id = source.id,
                Author = new RemarkAuthorDto
                {
                    UserId = source.author.userId,
                    Name = source.author.name
                },
                Category = new RemarkCategoryDto
                {
                    Id = source.category.id,
                    Name = source.category.name
                },
                Photo = new FileDto
                {
                    FileId = source.photo.fileId,
                    Name = source.photo.name,
                    ContentType = source.photo.contentType
                },
                Location = new LocationDto
                {
                    Address = source.location.address,
                    Coordinates = new[]
                    {
                        (double) source.location.coordinates[0],
                        (double) source.location.coordinates[1]
                    },
                    Type = source.location.type
                },
                Description = source.description,
                Resolved = source.resolved,
                ResolvedAt = source.resolvedAt,
                CreatedAt = source.createdAt
            };
        }
    }
}