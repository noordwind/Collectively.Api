namespace Coolector.Common.Commands.Users
{
    public class EditUser : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}