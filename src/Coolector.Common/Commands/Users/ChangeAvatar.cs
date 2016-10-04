namespace Coolector.Common.Commands.Users
{
    public class ChangeAvatar : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public string PictureUrl { get; set; }
    }
}