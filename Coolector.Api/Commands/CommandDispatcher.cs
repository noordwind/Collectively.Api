using System;
using System.Threading.Tasks;
using Coolector.Common.Commands;
using RawRabbit;

namespace Coolector.Api.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IBusClient _bus;

        public CommandDispatcher(IBusClient bus)
        {
            _bus = bus;
        }

        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "Command can not be null.");

            await _bus.PublishAsync(command);
        }
    }
}