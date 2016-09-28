namespace Coolector.Common.Events.Users
{
    public class UserSignedIn : IEvent
    {
        public string Email { get; }

        public UserSignedIn(string email)
        {
            Email = email;
        }
    }
}