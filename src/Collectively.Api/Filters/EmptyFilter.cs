using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Queries;
using Collectively.Common.Types;

namespace Collectively.Api.Filters
{
    public class EmptyFilter<TResult, TQuery> : IFilter<TResult, TQuery> where TQuery : IQuery
    {
        public IEnumerable<TResult> Filter(IEnumerable<TResult> values, TQuery query)
            => values ?? Enumerable.Empty<TResult>();
    }
}