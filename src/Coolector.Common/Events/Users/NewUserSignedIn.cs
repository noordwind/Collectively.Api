using System;

namespace Coolector.Common.Events.Users
{
    public class NewUserSignedIn : IEvent
    {
        public string UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public string PictureUrl { get; }
        public string Role { get; }
        public string State { get; }
        public DateTime CreatedAt { get; }

        protected NewUserSignedIn()
        {
        }

        public NewUserSignedIn(string userId, string email, string name, 
            string pictureUrl, string role, string state, DateTime createdAt)
        {
            UserId = userId;
            Email = email;
            Name = name;
            PictureUrl = pictureUrl;
            Role = role;
            State = state;
            CreatedAt = createdAt;
        }
    }
}