using System;
using Coolector.Common.Queries;

namespace Coolector.Core.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}