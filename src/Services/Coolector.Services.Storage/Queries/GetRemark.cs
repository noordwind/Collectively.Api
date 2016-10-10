using System;
using Coolector.Common.Queries;

namespace Coolector.Services.Storage.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}