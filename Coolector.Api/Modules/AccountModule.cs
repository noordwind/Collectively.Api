using Coolector.Api.Validation;
using Coolector.Common.Commands.Users;
using Coolector.Dto.Users;
using Nancy;
using GetAccount = Coolector.Api.Queries.GetAccount;
using GetAccoutByName = Coolector.Api.Queries.GetAccoutByName;
using ICommandDispatcher = Coolector.Api.Commands.ICommandDispatcher;
using IUserStorage = Coolector.Api.Storages.IUserStorage;

namespace Coolector.Api.Modules
{
    public class AccountModule : ModuleBase
    {
        public AccountModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IUserStorage userStorage)
            : base(commandDispatcher, validatorResolver)
        {
            Get("account", async args => await Fetch<GetAccount, UserDto>
                (async x => await userStorage.GetAsync(x.UserId)).HandleAsync());

            Get("{name}/account", async args => await Fetch<GetAccoutByName, UserDto>
                (async x => await userStorage.GetByNameAsync(x.Name)).HandleAsync());

            Post("sign-in", async args => await For<SignInUser>()
                .OnSuccess(HttpStatusCode.NoContent)
                .DispatchAsync());

            Put("account/username", async args => await For<ChangeUserName>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Put("account/avatar", async args => await For<ChangeAvatar>()
                .OnSuccessAccepted("account")
                .DispatchAsync());
        }
    }
}