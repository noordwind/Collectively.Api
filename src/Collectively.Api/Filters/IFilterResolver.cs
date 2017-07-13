using Collectively.Common.Queries;
using Collectively.Common.Types;

namespace Collectively.Api.Filters
{
    public interface IFilterResolver
    {
        IFilter<TResult, TQuery> Resolve<TResult, TQuery>() where TQuery : IQuery;
    }
}