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

                //TODO: Extract this to the remarks service.
                //var startIndex = command.Base64File.IndexOf(",") + 1;
                //var base64String = command.Base64File.Substring(startIndex);
                //var imageBytes = Convert.FromBase64String(base64String);
                await CommandDispatcher.DispatchAsync(command);
            });
        }
    }
}