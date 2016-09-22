using System.Threading.Tasks;
using Coolector.Core.Events.Users;
using Coolector.Infrastructure.Services;

namespace Coolector.Infrastructure.Events.Users
{
    public class NewUserSignedInHandler : IEventHandler<NewUserSignedIn>
    {
        private readonly IUserService _userService;

        public NewUserSignedInHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(NewUserSignedIn @event)
        {
            await _userService.CreateAsync(@event.Email, @event.ExternalId);
        }
    }
}