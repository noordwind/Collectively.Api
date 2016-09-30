using System;

namespace Coolector.Core.Storages
{
    public class StorageSettings
    {
        public string Url { get; set; }
        public TimeSpan? CacheExpiry { get; set; }
    }
}