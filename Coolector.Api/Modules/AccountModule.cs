using Coolector.Api.Queries;
using Coolector.Api.Validation;
using Coolector.Common.Commands.Users;
using Coolector.Dto.Common;
using Coolector.Dto.Users;
using GetAccount = Coolector.Api.Queries.GetAccount;
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

            Get("{name}/account", async args => await Fetch<GetAccountByName, UserDto>
                (async x => await userStorage.GetByNameAsync(x.Name)).HandleAsync());

            Get("{name}/available", async args => await Fetch<GetNameAvailability, AvailableResourceDto>
                (async x => await userStorage.IsNameAvailableAsync(x.Name)).HandleAsync());

            Put("account/username", async args => await For<ChangeUserName>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Put("account/avatar", async args => await For<ChangeAvatar>()
                .OnSuccessAccepted("account")
                .DispatchAsync());
        }
    }
}