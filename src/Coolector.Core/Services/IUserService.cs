using System.Threading.Tasks;
using Coolector.Common.Events.Users;
using Coolector.Common.Extensions;
using Coolector.Core.Domain;
using Coolector.Core.Domain.Users;
using Coolector.Core.Events;
using Coolector.Core.Repositories;

namespace Coolector.Core.Services
{
    public interface IUserService
    {
        Task SignInUserAsync(string email, string externalId, string picture);
        Task CreateAsync(string email, string externalId);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IEventDispatcher _eventDispatcher;

        public UserService(IUserRepository repository, IEventDispatcher eventDispatcher)
        {
            _repository = repository;
            _eventDispatcher = eventDispatcher;
        }

        public async Task SignInUserAsync(string email, string externalId, string picture)
        {
            if (email.Empty())
                throw new ServiceException("Email cannot be empty");
            if (externalId.Empty())
                throw new ServiceException("ExternalId cannot be empty");

            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
            {
                await _eventDispatcher.DispatchAsync(new NewUserSignedUp(email, externalId, picture));
                return;
            }

            await _eventDispatcher.DispatchAsync(new UserSignedIn(email));
        }

        public async Task CreateAsync(string email, string externalId)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user != null)
                throw new ServiceException($"User with e-mail: {email} already exists!");

            user = new User(email, externalId: externalId);
            await _repository.AddAsync(user);
            await _eventDispatcher.DispatchAsync(new UserCreated(user.Id, user.Email));
        }
    }
}