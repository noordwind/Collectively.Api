namespace Coolector.Common.Commands
{
    public interface IAuthenticatedCommand : ICommand
    {
        string UserId { get; set; }
    }
}