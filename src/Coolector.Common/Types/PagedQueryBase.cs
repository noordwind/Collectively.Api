namespace Coolector.Common.Types
{
    public abstract class PagedQueryBase : IQuery
    {
        public int Page { get; set; }
        public int Results { get; set; }
    }
}