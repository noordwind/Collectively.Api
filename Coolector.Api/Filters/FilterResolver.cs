using Autofac;
using Coolector.Common.Queries;
using Coolector.Common.Types;

namespace Coolector.Api.Filters
{
    public class FilterResolver : IFilterResolver
    {
        private readonly IComponentContext _context;

        public FilterResolver(IComponentContext context)
        {
            _context = context;
        }

        public IFilter<TResult, TQuery> Resolve<TResult, TQuery>() where TQuery : IQuery
        {
            IFilter<TResult, TQuery> filter;

            return _context.TryResolve(out filter) ? filter : new EmptyFilter<TResult, TQuery>();
        }
    }
}