namespace Coolector.Common.Events.Users
{
    public class AvatarChanged : IEvent
    {
        public string UserId { get; set; }
        public string PictureUrl { get; set; }

        public AvatarChanged(string userId, string pictureUrl)
        {
            UserId = userId;
            PictureUrl = pictureUrl;
        }
    }
}