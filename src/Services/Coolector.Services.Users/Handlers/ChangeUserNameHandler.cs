using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Users;
using Coolector.Common.Events.Users;
using Coolector.Services.Users.Services;
using RawRabbit;

namespace Coolector.Services.Users.Handlers
{
    public class ChangeUserNameHandler : ICommandHandler<ChangeUserName>
    {
        private readonly IBusClient _bus;
        private readonly IUserService _userService;

        public ChangeUserNameHandler(IBusClient bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        public async Task HandleAsync(ChangeUserName command)
        {
            await _userService.ChangeNameAsync(command.UserId, command.Name);
            await _bus.PublishAsync(new UserNameChanged(command.UserId, command.Name));
        }
    }
}