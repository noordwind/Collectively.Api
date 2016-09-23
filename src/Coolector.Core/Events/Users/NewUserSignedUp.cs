namespace Coolector.Core.Events.Users
{
    public class NewUserSignedUp : IEvent
    {
        public string Email { get; }
        public string ExternalId { get; }
        public string Picture { get; }

        public NewUserSignedUp(string email, string externalId, string picture)
        {
            Email = email;
            ExternalId = externalId;
            Picture = picture;
        }
    }
}