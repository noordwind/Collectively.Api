using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Notifications;
using Collectively.Messages.Commands.Users;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Modules
{
    public class AccountModule : ModuleBase
    {
        public AccountModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IUserStorage userStorage,
            INotificationSettingsStorage notificationSettingsStorage)
            : base(commandDispatcher, validatorResolver)
        {
            Get("account", async args => await Fetch<GetAccount, User>
                (async x => await userStorage.GetAsync(x.UserId)).HandleAsync());

            Get("account/settings/notifications", async args => await Fetch<GetNotificationSettings, UserNotificationSettings>
                (async x => await notificationSettingsStorage.GetAsync(x.UserId))
                .HandleAsync());

            Get("account/names/{name}/available", async args => await Fetch<GetNameAvailability, AvailableResource>
                (async x => await userStorage.IsNameAvailableAsync(x.Name)).HandleAsync());

            Put("account/name", async args => await For<ChangeUsername>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Put("account/settings/notifications", async args => await For<UpdateUserNotificationSettings>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Post("account/avatar", async args => await For<UploadAvatar>()
                .OnSuccessAccepted("account")
                .DispatchAsync());

            Delete("account/avatar", async args => await For<RemoveAvatar>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Put("account/password", async args => await For<ChangePassword>()
                .OnSuccessAccepted("account")
                .DispatchAsync());
        }
    }
}