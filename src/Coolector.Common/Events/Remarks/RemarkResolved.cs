using System;
using Coolector.Common.Events.Remarks.Models;

namespace Coolector.Common.Events.Remarks
{
    public class RemarkResolved : IEvent
    {
        public Guid RemarkId { get; protected set; }
        public string UserId { get; protected set; }
        public RemarkFile Photo { get; protected set; }

        public RemarkResolved(Guid remarkId, string userId, RemarkFile photo)
        {
            RemarkId = remarkId;
            UserId = userId;
            Photo = photo;
        }
    }
}