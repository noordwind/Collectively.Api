namespace Coolector.Services.Domain
{
    public abstract class PagedQueryBase : IQuery
    {
        public int Page { get; set; }
        public int Results { get; set; }
    }
}