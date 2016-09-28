using System.Collections.Generic;
using Coolector.Common.Events;

namespace Coolector.Services.Domain
{
    public interface IEntity
    {
        IEnumerable<IEvent> Events { get; }
    }
}