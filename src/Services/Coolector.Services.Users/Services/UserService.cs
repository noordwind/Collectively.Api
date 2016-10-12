using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Services.Domain;
using Coolector.Services.Users.Domain;
using Coolector.Services.Users.Queries;
using Coolector.Services.Users.Repositories;

namespace Coolector.Services.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Maybe<User>> GetAsync(string userId)
            => await _userRepository.GetByUserIdAsync(userId);

        public async Task<Maybe<User>> GetByNameAsync(string name)
            => await _userRepository.GetByNameAsync(name);

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query)
            => await _userRepository.BrowseAsync(query);

        public async Task CreateAsync(string userId, string email, string role,
            bool activate = true, string pictureUrl = null, string name = null)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasValue)
                throw new ServiceException($"User with id: {userId} already exists!");

            user = new User(userId, email, role, pictureUrl);
            if (activate)
                user.Value.Activate();

            user.Value.SetName(name.Empty() ? $"user-{user.Value.Id:N}" : name);

            await _userRepository.AddAsync(user.Value);
        }

        public async Task ChangeNameAsync(string userId, string name)
        {
            var user = await GetAsync(userId);
            if (user.HasNoValue)
                throw new ServiceException($"User with id {userId} has not been found.");

            user.Value.SetName(name);
            await _userRepository.UpdateAsync(user.Value);
        }

        public async Task ChangeAvatarAsync(string userId, string pictureUrl)
        {
            var user = await GetAsync(userId);
            if (user.HasNoValue)
                throw new ServiceException($"User with id {userId} has not been found.");

            user.Value.SetAvatar(pictureUrl);
            await _userRepository.UpdateAsync(user.Value);
        }
    }
}