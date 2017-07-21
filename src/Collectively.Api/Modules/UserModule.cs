using Collectively.Api.Queries;
using Collectively.Api.Validation;
using Collectively.Services.Storage.Models.Users;
using BrowseUsers = Collectively.Api.Queries.BrowseUsers;
using ICommandDispatcher = Collectively.Api.Commands.ICommandDispatcher;
using IUserStorage = Collectively.Api.Storages.IUserStorage;

namespace Collectively.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(ICommandDispatcher commandDispatcher,
            IUserStorage userStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, UserInfo>
                (async x => await userStorage.BrowseAsync(x)).HandleAsync());

            Get("{name}", async args => await Fetch<GetUserByName, UserInfo>
                (async x => await userStorage.GetInfoByNameAsync(x.Name)).HandleAsync());
        }
    }
}