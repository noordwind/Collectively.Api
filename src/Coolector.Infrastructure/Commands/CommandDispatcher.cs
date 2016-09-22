using System.Threading.Tasks;
using Autofac;
using Coolector.Core.Domain;

namespace Coolector.Infrastructure.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command == null)
                throw new ServiceException("Command can not be null.");

            ICommandHandler<T> commandHandler;
            if (!_context.TryResolve(out commandHandler))
            {
                throw new ServiceException("ICommandHandler has not been found " +
                                           "for request: {0}.", command.GetType().Name);
            }

            await commandHandler.HandleAsync(command);
        }
    }
}