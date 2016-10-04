using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Commands.Remarks;

namespace Coolector.Services.Remarks.Handlers
{
    public class CreaterRemarkHandler : ICommandHandler<CreateRemark>
    {
        public async Task HandleAsync(CreateRemark command)
        {
        }
    }
}