using System;

namespace Coolector.Services.Domain
{
    public abstract class IdentifiableEntity : Entity, IIdentifiable
    {
        public Guid Id { get; protected set; }

        protected IdentifiableEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}