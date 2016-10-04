using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Services.Domain;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class UserNameChangedHandler : IEventHandler<UserNameChanged>
    {
        private readonly IUserRepository _userRepository;

        public UserNameChangedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UserNameChanged @event)
        {
            var user = await _userRepository.GetByIdAsync(@event.UserId);
            if (user.HasNoValue)
                throw new ServiceException($"User name cannot be changed because user: {@event.UserId} does not exist");

            user.Value.Name = @event.NewName;
            await _userRepository.EditAsync(user.Value);
        }
    }
}