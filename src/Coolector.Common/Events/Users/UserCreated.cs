using System;
using Coolector.Common.DTO.Common;
using Coolector.Common.DTO.Users;

namespace Coolector.Common.Events.Users
{
    public class UserCreated : IEvent
    {
        public string UserId { get; }
        public string Email { get; }
        public Role Role { get; }
        public State State { get; }
        public DateTime CreatedAt { get; }

        protected UserCreated()
        {
        }

        public UserCreated(string userId, string email,
            Role role, State state, DateTime createdAt)
        {
            UserId = userId;
            Email = email;
            Role = role;
            State = state;
            CreatedAt = createdAt;
        }
    }
}