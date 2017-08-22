using Collectively.Api.Commands;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Remarks;

namespace Collectively.Api.Modules
{
    public class RemarkReportModule : ModuleBase
    {
        public RemarkReportModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "remarks/{remarkId}/reports")
        {
            Post("", async args => await For<ReportRemark>()
                .OnSuccessAccepted()
                .DispatchAsync());
        }
    }
}