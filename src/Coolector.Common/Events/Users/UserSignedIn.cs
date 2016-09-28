namespace Coolector.Common.Events.Users
{
    public class UserSignedIn : IEvent
    {
        public string UserId { get; }
        public string Email { get; }

        protected UserSignedIn()
        {
        }

        public UserSignedIn(string userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}