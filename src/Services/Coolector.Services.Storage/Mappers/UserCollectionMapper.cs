using Coolector.Common.DTO.Users;

namespace Coolector.Services.Storage.Mappers
{
    public class UserCollectionMapper : CollectionMapper<UserDto>
    {
        public UserCollectionMapper(IMapper<UserDto> mapper) : base(mapper)
        {
        }
    }
}