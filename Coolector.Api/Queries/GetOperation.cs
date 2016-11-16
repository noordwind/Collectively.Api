using System;
using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetOperation : IQuery
    {
        public Guid RequestId { get; set; }
    }
}