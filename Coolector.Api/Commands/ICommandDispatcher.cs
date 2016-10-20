using System.Threading.Tasks;
using Coolector.Common.Commands;

namespace Coolector.Api.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
    }
}