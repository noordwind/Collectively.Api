using Coolector.Common.Commands;

namespace Coolector.Core.Commands.Users
{
    public class SignInUser : ICommand
    {
        public string AccessToken { get; set; }
    }
}