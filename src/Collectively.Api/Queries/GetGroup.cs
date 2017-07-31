using System;
using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetGroup : IQuery
    {
        public Guid Id { get; set; }
    }
}