using System;
using Coolector.Services.Domain;
using Coolector.Common.Extensions;

namespace Coolector.Services.Remarks.Domain
{
    public class User : IdentifiableEntity, ITimestampable
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public User(string userId, string name)
        {
            UserId = userId;
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (name.Empty())
                throw new ArgumentException("User name can not be empty.", nameof(name));
            if (name.Length > 50)
                throw new ArgumentException("User name is too long.", nameof(name));
            if (Name.EqualsCaseInvariant(name))
                return;

            Name = name.ToLowerInvariant();
        }
    }
}