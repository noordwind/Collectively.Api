using System.Collections.Generic;

namespace Coolector.Common.Types
{
    public interface IFilter<TResult, in TQuery> where TQuery : PagedQueryBase
    {
        Maybe<PagedResult<TResult>> Filter(Maybe<IEnumerable<TResult>> values, TQuery query);
    }
}