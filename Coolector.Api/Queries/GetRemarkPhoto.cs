using System;
using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetRemarkPhoto : IQuery
    {
        public Guid Id { get; set; }
    }
}