using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
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

        public async Task<Maybe<UserDto>> GetAsync(string userId)
        {
            var user = await _userRepository.GetByUserIdAsync(GetFixedUserId(userId));
            if (user.HasNoValue)
                return new Maybe<UserDto>();

            return new UserDto
            {
                UserId = user.Value.UserId,
                Email = user.Value.Email
            };
        }

        public async Task CreateAsync(string userId, string email, Role role = Role.User, bool activate = true)
        {
            userId = GetFixedUserId(userId);
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasValue)
                throw new ServiceException($"User with id: {userId} already exists!");

            user = new User(userId, email, role);
            if (activate)
                user.Value.Activate();
            await _userRepository.AddAsync(user.Value);
        }

        private static string GetFixedUserId(string userId)
            => userId.Empty() ? string.Empty : userId.Replace("auth0|", string.Empty);
    }
}