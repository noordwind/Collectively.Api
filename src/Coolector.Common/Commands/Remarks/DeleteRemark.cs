using System;

namespace Coolector.Common.Commands.Remarks
{
    public class DeleteRemark : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public Guid RemarkId { get; set; }
    }
}