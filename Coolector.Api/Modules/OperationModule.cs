using Coolector.Api.Commands;
using Coolector.Api.Queries;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using Coolector.Common.Dto.Operations;
using Coolector.Common.Types;
using Coolector.Common.Extensions;
using Nancy.Security;

namespace Coolector.Api.Modules
{
    public class OperationModule : ModuleBase
    {
        public OperationModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IOperationStorage operationStorage)
            : base(commandDispatcher, validatorResolver, modulePath: "operations")
        {
            Get("{requestId}", args => Fetch<GetOperation, OperationDto>
            (async x =>
            {
                var operation = await operationStorage.GetAsync(x.RequestId);
                if (operation.HasNoValue || operation.Value.UserId.Empty())
                    return operation;

                this.RequiresAuthentication();

                return operation.Value.UserId == CurrentUserId
                    ? operation
                    : new Maybe<OperationDto>();
            }).HandleAsync());
        }
    }
}