using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Core.Services;

namespace Coolector.Core.Events.Users
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