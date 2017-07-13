using System;
using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetOperation : IQuery
    {
        public Guid RequestId { get; set; }
    }
}