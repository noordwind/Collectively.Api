using Coolector.Common.Queries;

namespace Coolector.Common.Types
{
    public abstract class PagedQueryBase : IPagedQuery
    {
        public int Page { get; set; }
        public int Results { get; set; }
    }
}