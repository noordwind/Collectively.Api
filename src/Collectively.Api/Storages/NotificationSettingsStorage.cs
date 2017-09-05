using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Api.Storages
{

    public class NotificationSettingsStorage : INotificationSettingsStorage
    {
        private readonly IStorageClient _storageClient;
        private readonly ICache _cache;

        public NotificationSettingsStorage(IStorageClient storageClient,
            ICache cache)
        {
            _storageClient = storageClient;
            _cache = cache;
        }

        public async Task<Maybe<UserNotificationSettings>> GetAsync(string userId)
        {
            var settings = await _cache.GetAsync<UserNotificationSettings>($"users:{userId}:notifications:settings");
            if (settings.HasValue)
            {
                return settings;
            }

            return await _storageClient.GetAsync<UserNotificationSettings>($"notification/settings/{userId}");
        }
    }
}