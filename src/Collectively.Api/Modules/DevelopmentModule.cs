using System;
using Collectively.Api.Commands;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Services.Storage.Models.Operations;
using Nancy;

namespace Collectively.Api.Modules
{
    public class DevelopmentModule : ModuleBase
    {
        private static int RequestCounter = 0;

        public DevelopmentModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IOperationStorage operationStorage)
            : base(commandDispatcher, validatorResolver, modulePath: "development")
        {
            Get("operation", args => 
            {
                RequestCounter++;
                if(RequestCounter <= 2)
                {
                    return HttpStatusCode.NotFound;
                }
                if(RequestCounter == 3)
                {
                    return HttpStatusCode.Unauthorized;
                }
                if(RequestCounter == 8)
                {
                    RequestCounter = 0;
                }
                return new Operation
                {
                    Id = Guid.NewGuid(),
                    RequestId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    State = "completed"
                };
            });
        }
    }
}