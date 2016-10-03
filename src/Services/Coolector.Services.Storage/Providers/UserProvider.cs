using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Mappers;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;
using Coolector.Dto.Users;

namespace Coolector.Services.Storage.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IProviderClient _providerClient;
        private readonly ProviderSettings _providerSettings;
        private readonly IMapper<UserDto> _mapper;
        private readonly IMapper<IEnumerable<UserDto>> _collectionMapper;

        public UserProvider(IUserRepository userRepository,
            IProviderClient providerClient,
            ProviderSettings providerSettings)
        {
            _userRepository = userRepository;
            _providerClient = providerClient;
            _providerSettings = providerSettings;
        }

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
            => await _providerClient.GetCollectionUsingStorageAsync(_providerSettings.UsersApiUrl, "users",
                async () => await _userRepository.BrowseAsync(query),
                async users => await _userRepository.AddManyAsync(users.Items));

        public async Task<Maybe<UserDto>> GetAsync(string userId)
            => await _providerClient.GetUsingStorageAsync(_providerSettings.UsersApiUrl, $"users/{userId}",
                async () =>  await _userRepository.GetByIdAsync(userId),
                async user => await _userRepository.AddAsync(user));
    }
}