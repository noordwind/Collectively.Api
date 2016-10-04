using System;
using System.Linq;
using Coolector.Api.Modules.Base;
using Coolector.Common.Commands.Remarks;
using Coolector.Core.Commands;

namespace Coolector.Api.Modules
{
    public class RemarkModule : AuthenticatedModule
    {
        public RemarkModule(ICommandDispatcher commandDispatcher)
            :base(commandDispatcher, modulePath: "remarks")
        {
            Post("", async args =>
            {
                var command = BindAuthenticatedCommand<CreateRemark>();
                await CommandDispatcher.DispatchAsync(command);
            });
        }
    }
}