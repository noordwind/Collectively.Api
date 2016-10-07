using System;

namespace Coolector.Common.Events.Remarks
{
    public class RemarkDeleted : IEvent
    {
        public Guid Id { get; set; }

        public RemarkDeleted(Guid id)
        {
            Id = id;
        }
    }
}