using System.Threading.Tasks;
using Collectively.Common.Commands;

namespace Collectively.Api.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
    }
}