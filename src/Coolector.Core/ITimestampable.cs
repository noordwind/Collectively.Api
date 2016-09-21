using System;

namespace Coolector.Core
{
    public interface ITimestampable
    {
        DateTime CreatedAt { get; }
    }
}