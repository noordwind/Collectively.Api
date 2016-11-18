using System;

namespace Coolector.Api.Storages
{
    public class StorageSettings
    {
        public string Url { get; set; }
        public TimeSpan? CacheExpiry { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelayMilliseconds { get; set; }
    }
}