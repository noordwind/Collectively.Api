using System.Threading.Tasks;
using Collectively.Api.Framework;
using Collectively.Api.Queries;
using Collectively.Api.Validation;
using Collectively.Common.Extensions;
using Collectively.Messages.Commands.Users;
using Collectively.Services.Storage.Models.Users;
using Nancy;
using BrowseUsers = Collectively.Api.Queries.BrowseUsers;
using ICommandDispatcher = Collectively.Api.Commands.ICommandDispatcher;
using IUserStorage = Collectively.Api.Storages.IUserStorage;

namespace Collectively.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(ICommandDispatcher commandDispatcher,
            IUserStorage userStorage,
            IValidatorResolver validatorResolver,
            AppSettings settings)
            : base(commandDispatcher, validatorResolver, modulePath: "users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, UserInfo>
                (async x => await userStorage.BrowseAsync(x)).HandleAsync());

            Get("{name}", async args => await Fetch<GetUserByName, UserInfo>
                (async x => await userStorage.GetInfoByNameAsync(x.Name)).HandleAsync());

            Get("{id}/avatar", args => 
            {
                var query = BindRequest<GetAvatar>();
                if (query.Id.Empty())
                {
                    return HttpStatusCode.NotFound;
                }

                return Response.AsRedirect(string.Format(settings.AvatarUrl, query.Id));
            });

            Put("{lockUserId}/lock", async args => await ForAdministrator<LockAccount>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Put("{unlockUserId}/unlock", async args => await ForAdministrator<UnlockAccount>()
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}