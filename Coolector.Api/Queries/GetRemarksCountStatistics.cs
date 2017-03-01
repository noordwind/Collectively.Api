using System;
using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetRemarksCountStatistics : IQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}