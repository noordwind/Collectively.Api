using System.Threading.Tasks;

namespace Coolector.Infrastructure.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}