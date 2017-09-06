using Collectively.Common.Types;

namespace Collectively.Api.Queries
{
    public abstract class BrowseRemarksBase : PagedQueryBase
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public bool SkipLocation { get; set; }
    }
}