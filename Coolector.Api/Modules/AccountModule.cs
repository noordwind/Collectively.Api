using Coolector.Api.Commands;
using Coolector.Api.Queries;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using Coolector.Common.Dto.General;
using Coolector.Services.Users.Shared.Commands;
using Coolector.Services.Users.Shared.Dto;

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

            Get("account/names/{name}/available", async args => await Fetch<GetNameAvailability, AvailableResourceDto>
                (async x => await userStorage.IsNameAvailableAsync(x.Name)).HandleAsync());

            Put("account/name", async args => await For<ChangeUserName>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Put("account/avatar", async args => await For<ChangeAvatar>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Put("account/password", async args => await For<ChangePassword>()
                .OnSuccessAccepted("account")
                .DispatchAsync());
        }
    }
}