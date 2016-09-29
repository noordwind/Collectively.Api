using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Types;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;

namespace Coolector.Services.Storage.Providers
{
    public interface IUserProvider
    {
        Task<Maybe<UserDto>> GetAsync(string userId);
    }

    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IProviderClient _providerClient;
        private readonly ProviderSettings _providerSettings;

        public UserProvider(IUserRepository userRepository,
            IProviderClient providerClient,
            ProviderSettings providerSettings)
        {
            _userRepository = userRepository;
            _providerClient = providerClient;
            _providerSettings = providerSettings;
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
                });
    }
}