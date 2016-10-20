using Coolector.Common.Queries;
using Coolector.Common.Types;

namespace Coolector.Api.Filters
{
    public interface IFilterResolver
    {
        IFilter<TResult, TQuery> Resolve<TResult, TQuery>() where TQuery : IQuery;
    }
}