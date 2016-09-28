using System;
using Coolector.Common.Extensions;

namespace Coolector.Core.Domain.Users
{
    public class User : Entity, ITimestampable
    {
        public string ExternalId { get; set; }
        public string Email { get; protected set; }
        public string Name { get; protected set; }
        public Role Role { get; protected set; }
        public State State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected User()
        {
        }

        public User(string email, Role role = Role.User, string externalId = null)
        {
            SetEmail(email);
            Role = role;
            State = State.Inactive;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ExternalId = externalId;
        }

        public void SetEmail(string email)
        {
            if (email.Empty())
                throw new ArgumentException("Email can not be empty.", nameof(email));
            if (!email.IsEmail())
                throw new ArgumentException($"Invalid email {email}.", nameof(email));
            if (Email.EqualsCaseInvariant(email))
                return;

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
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
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetExternalId(string externalId)
        {
            if (externalId.Empty())
                throw new ArgumentException("External id can not be empty.", nameof(externalId));

            ExternalId = externalId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRole(Role role)
        {
            if (Role == role)
                return;

            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Lock()
        {
            if (State == State.Locked)
                return;

            State = State.Locked;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (State == State.Active)
                return;

            State = State.Active;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}