using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Services.Domain;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class AvatarChangedHandler : IEventHandler<AvatarChanged>
    {
        private readonly IUserRepository _userRepository;

        public AvatarChangedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AvatarChanged @event)
        {
            var user = await _userRepository.GetByIdAsync(@event.UserId);
            if (user.HasNoValue)
                throw new ServiceException($"Avatar cannot be changed because user: {@event.UserId} does not exist");

            user.Value.PictureUrl = @event.PictureUrl;
            await _userRepository.EditAsync(user.Value);
        }
    }
}