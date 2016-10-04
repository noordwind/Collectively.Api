using Coolector.Dto.Remarks;

namespace Coolector.Services.Storage.Mappers
{
    public class RemarkCollectionMapper : CollectionMapper<RemarkDto>
    {
        public RemarkCollectionMapper(IMapper<RemarkDto> mapper) : base(mapper)
        {
        }
    }
}