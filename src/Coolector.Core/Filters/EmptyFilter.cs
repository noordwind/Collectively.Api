using System;
using System.Collections.Generic;
using System.Linq;
using Coolector.Common.Queries;
using Coolector.Common.Types;

namespace Coolector.Core.Filters
{
    public class EmptyFilter<TResult, TQuery> : IFilter<TResult, TQuery> where TQuery : IQuery
    {
        public IEnumerable<TResult> Filter(IEnumerable<TResult> values, TQuery query)
            => values ?? Enumerable.Empty<TResult>();
    }
}