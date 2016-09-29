using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Services.Domain;
using Coolector.Services.Users.Domain;
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
            => await _userRepository.GetByUserIdAsync(GetFixedUserId(userId));

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(int page = 1, int results = 10)
            => await _userRepository.BrowseAsync(page, results);

        public async Task CreateAsync(string userId, string email, string role,
            bool activate = true, string pictureUrl = null)
        {
            userId = GetFixedUserId(userId);
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasValue)
                throw new ServiceException($"User with id: {userId} already exists!");

            user = new User(userId, email, role, pictureUrl);
            if (activate)
                user.Value.Activate();

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

        private static string GetFixedUserId(string userId)
            => userId.Empty() ? string.Empty : userId.Replace("auth0|", string.Empty);
    }
}