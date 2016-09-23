using System.Threading.Tasks;
using Coolector.Core.Events.Users;
using Coolector.Infrastructure.Services;

namespace Coolector.Infrastructure.Events.Users
{
    public class NewUserSignedUpHandler : IEventHandler<NewUserSignedUp>
    {
        private readonly IUserService _userService;

        public NewUserSignedUpHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(NewUserSignedUp @event)
        {
            await _userService.CreateAsync(@event.Email, @event.ExternalId);
        }
    }
}