using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Users;
using Coolector.Common.Events.Users;
using RawRabbit;

namespace Coolector.Services.Users.Handlers
{
    public class SignInUserHandler : ICommandHandler<SignInUser>
    {
        private readonly IBusClient _bus;

        public SignInUserHandler(IBusClient bus)
        {
            _bus = bus;
        }

        public async Task HandleAsync(SignInUser command)
        {
            await _bus.PublishAsync(new UserSignedIn("a", "b"));
        }
    }
}