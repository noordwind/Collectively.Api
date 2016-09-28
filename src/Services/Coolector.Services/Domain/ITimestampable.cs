using System;

namespace Coolector.Services.Domain
{
    public interface ITimestampable
    {
        DateTime CreatedAt { get; }
    }
}