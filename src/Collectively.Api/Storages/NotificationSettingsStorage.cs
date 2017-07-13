using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Api.Storages
{
    public class NotificationSettingsStorage : INotificationSettingsStorage
    {
        private readonly IStorageClient _storageClient;

        public NotificationSettingsStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<UserNotificationSettings>> GetAsync(string userId)
            => await _storageClient.GetAsync<UserNotificationSettings>($"notification/settings/{userId}");
    }
}