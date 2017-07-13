using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Api.Storages
{
    public interface INotificationSettingsStorage
    {
        Task<Maybe<UserNotificationSettings>> GetAsync(string userId);
    }
}