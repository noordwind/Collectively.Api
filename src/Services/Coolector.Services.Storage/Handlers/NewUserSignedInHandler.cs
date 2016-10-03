using System.Threading.Tasks;
using Coolector.Common.DTO.Users;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class NewUserSignedInHandler : IEventHandler<NewUserSignedIn>
    {
        private readonly IUserRepository _repository;

        public NewUserSignedInHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(NewUserSignedIn @event)
        {
            if (await _repository.ExisitsAsync(@event.UserId))
                return;

            var user = new UserDto
            {
                UserId = @event.UserId,
                Name = @event.Name,
                Email = @event.Email,
                CreatedAt = @event.CreatedAt,
                PictureUrl = @event.PictureUrl,
                Role = @event.Role
            };
            await _repository.AddAsync(user);
        }
    }
}