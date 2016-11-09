using System;
using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetOperation : IAuthenticatedQuery
    {
        public Guid RequestId { get; set; }
        public string UserId { get; set; }
    }
}