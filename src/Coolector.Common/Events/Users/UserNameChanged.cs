namespace Coolector.Common.Events.Users
{
    public class UserNameChanged : IAuthenticatedEvent
    {
        public string UserId { get; set; }
        public string NewName { get; }

        protected UserNameChanged()
        {
        }

        public UserNameChanged(string userId, string newName)
        {
            UserId = userId;
            NewName = newName;
        }
    }
}