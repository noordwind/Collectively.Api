using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using System.Linq;
using Collectively.Messages.Commands.Remarks;
using System.Collections.Generic;
using System;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Messages.Commands.Models;
using System.Threading.Tasks;
using Collectively.Messages.Commands;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Serialization;

namespace Collectively.Api.Modules
{
    public class RemarkAssignmentModule : ModuleBase
    {
        public RemarkAssignmentModule(ICommandDispatcher commandDispatcher,
            IRemarkStorage remarkStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "remarks/{remarkId}/assignments")
        {
            Post("", async args => await For<AssignRemarkToGroup>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Put("deny", async args => await For<DenyRemarkAssignment>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Delete("", async args => await For<RemoveRemarkAssignment>()
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}