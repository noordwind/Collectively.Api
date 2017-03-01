using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Common.Types;
using Collectively.Common.Extensions;

using Nancy.Security;

namespace Collectively.Api.Modules
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