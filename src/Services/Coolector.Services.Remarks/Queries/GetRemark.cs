using System;
using Coolector.Common.Queries;

namespace Coolector.Services.Remarks.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}