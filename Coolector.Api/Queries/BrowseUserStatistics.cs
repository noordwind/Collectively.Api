using Coolector.Common.Types;

namespace Coolector.Api.Queries
{
    public class BrowseUserStatistics : PagedQueryBase
    {
        public string OrderBy { get; set; }
    }
}