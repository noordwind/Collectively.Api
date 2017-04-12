using System;
using Collectively.Api.Commands;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Api.Modules
{
    public class DevelopmentModule : ModuleBase
    {
        public DevelopmentModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IOperationStorage operationStorage)
            : base(commandDispatcher, validatorResolver, modulePath: "development")
        {
            Get("operation/{state}", args => 
            {
                return new Operation
                {
                    Id = Guid.NewGuid(),
                    RequestId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    State = args.state
                };
            });
        }
    }
}