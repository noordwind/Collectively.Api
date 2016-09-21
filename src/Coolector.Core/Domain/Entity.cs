using System;
using System.Collections.Generic;

namespace Coolector.Core.Domain
{
    public abstract class Entity : IEntity
    {
        private readonly Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        public Guid Id { get; }
        public IEnumerable<IEvent> Events => _events.Values;

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

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