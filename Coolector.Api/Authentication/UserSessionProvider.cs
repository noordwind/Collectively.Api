using System;
using System.Threading.Tasks;
using Coolector.Api.Storages;
using Coolector.Common.Types;
using Coolector.Dto.Users;
using NLog;
using Polly;

namespace Coolector.Api.Authentication
{
    public class UserSessionProvider : IUserSessionProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUserStorage _userStorage;

        public UserSessionProvider(IUserStorage userStorage)
        {
            _userStorage = userStorage;
        }

        public async Task<Maybe<UserSessionDto>> GetAsync(Guid id)
        {
            var policy = Policy
                .Handle<TimeoutException>()
                .WaitAndRetryAsync(5, retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Logger.Error(exception, $"Cannot get user session with id: '{id}'. " +
                                                $"RetryCount:{retryCount}, duration:{timeSpan}");
                    });

            return await policy.ExecuteAsync(() => _userStorage.GetSessionAsync(id));
        }
    }
}