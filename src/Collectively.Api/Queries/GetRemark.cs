using System;
using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}