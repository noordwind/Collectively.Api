using System;
using Coolector.Common.DTO.Common;
using Coolector.Common.DTO.Users;
using Coolector.Common.Extensions;
using Coolector.Services.Domain;

namespace Coolector.Services.Users.Domain
{
    public class User : Entity, ITimestampable
    {
        public string UserId { get; set; }
        public string Email { get; protected set; }
        public string Name { get; protected set; }
        public Role Role { get; protected set; }
        public State State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected User()
        {
        }

        public User(string userId, string email, Role role = Role.User)
        {
            SetUserId(userId);
            SetEmail(email);
            Role = role;
            State = State.Inactive;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UserId = userId;
        }
        public void SetUserId(string userId)
        {
            if (userId.Empty())
                throw new ArgumentException("User id can not be empty.", nameof(userId));

            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
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