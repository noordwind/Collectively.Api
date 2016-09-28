using System;
using System.Collections.Generic;
using Coolector.Common.Events;

namespace Coolector.Services.Domain
{
    public abstract class Entity : IEntity
    {
        private readonly Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        public IEnumerable<IEvent> Events => _events.Values;

        protected void AddEvent(IEvent @event)
        {
            _events[@event.GetType()] = @event;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
