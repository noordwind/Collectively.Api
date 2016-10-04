using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Services.Remarks.Services;

namespace Coolector.Services.Remarks.Handlers
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
            await _userService.CreateAsyncIfNotFound(@event.UserId, @event.Name);
        }
    }
}