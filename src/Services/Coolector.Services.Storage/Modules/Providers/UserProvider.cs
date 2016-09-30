using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Services.Storage.Mappers;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;

namespace Coolector.Services.Storage.Modules.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IProviderClient _providerClient;
        private readonly ProviderSettings _providerSettings;
        private readonly IMapper<UserDto> _mapper;

        public UserProvider(IUserRepository userRepository,
            IProviderClient providerClient,
            ProviderSettings providerSettings,
            IMapper<UserDto> mapper)
        {
            _userRepository = userRepository;
            _providerClient = providerClient;
            _providerSettings = providerSettings;
            _mapper = mapper;
        }

        public async Task<Maybe<UserDto>> GetAsync(string userId)
            => await _providerClient.GetUsingStorageAsync(_providerSettings.UsersApiUrl, $"users/{userId}",
                async () =>
                {
                    var user = await _userRepository.GetByIdAsync(userId);

                    return user.HasValue ? user.Value : new Maybe<UserDto>();
                },
                async user =>
                {
                    await _userRepository.AddAsync(user);
                },
                _mapper);
    }
}