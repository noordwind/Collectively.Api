using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Users;
using Coolector.Common.Events.Users;
using Coolector.Services.Users.Services;
using RawRabbit;

namespace Coolector.Services.Users.Handlers
{
    public class ChangeAvatarHandler : ICommandHandler<ChangeAvatar>
    {
        private readonly IBusClient _bus;
        private readonly IUserService _userService;

        public ChangeAvatarHandler(IBusClient bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        public async Task HandleAsync(ChangeAvatar command)
        {
            await _userService.ChangeAvatarAsync(command.UserId, command.PictureUrl);
            await _bus.PublishAsync(new AvatarChanged(command.UserId, command.PictureUrl));
        }
    }
}