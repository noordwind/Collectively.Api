using System;
using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetRemarksCountStatistics : IQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}