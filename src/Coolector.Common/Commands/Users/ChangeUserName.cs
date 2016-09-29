namespace Coolector.Common.Commands.Users
{
    public class ChangeUserName : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}