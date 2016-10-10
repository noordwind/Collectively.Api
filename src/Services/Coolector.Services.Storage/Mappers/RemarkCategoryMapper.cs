using Coolector.Dto.Remarks;

namespace Coolector.Services.Storage.Mappers
{
    public class RemarkCategoryMapper : IMapper<RemarkCategoryDto>
    {
        public RemarkCategoryDto Map(dynamic source)
        {
            return new RemarkCategoryDto
            {
                Id = source.id,
                Name = source.name
            };
        }
    }
}