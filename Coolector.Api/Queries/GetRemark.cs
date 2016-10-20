using System;
using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}