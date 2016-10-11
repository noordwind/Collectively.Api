using Coolector.Common.Types;

namespace Coolector.Services.Remarks.Queries
{
    public class BrowseRemarks : PagedQueryBase
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
    }
}