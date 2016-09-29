namespace Coolector.Common.Events.Users
{
    public class UserSignedIn : IEvent
    {
        public string UserId { get; }
        public string Email { get; }
        public string Name { get; }

        protected UserSignedIn()
        {
        }

        public UserSignedIn(string userId, string email, string name)
        {
            UserId = userId;
            Email = email;
            Name = name;
        }
    }
}