using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Common.Types;
using Collectively.Common.Extensions;
using Collectively.Services.Storage.Models.Operations;
using Nancy.Security;
using Collectively.Api.Services;
using Nancy;

namespace Collectively.Api.Modules
{
    public class OperationModule : ModuleBase
    {
        public OperationModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IOperationProvider operationProvider)
            : base(commandDispatcher, validatorResolver, modulePath: "operations")
        {
            Get("{requestId}", args => Fetch<GetOperation, Operation>
            (async x => await operationProvider.GetAsync(x.RequestId)).HandleAsync());
        }
    }
}