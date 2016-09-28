using System;

namespace Coolector.Common.Events.Users
{
    public class UserCreated : IEvent
    {
        public Guid Id { get; }
        public string Email { get; }

        public UserCreated(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}