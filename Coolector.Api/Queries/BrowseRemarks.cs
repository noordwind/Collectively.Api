using System.Collections.Generic;
using Coolector.Common.Types;

namespace Coolector.Api.Queries
{
    public class BrowseRemarks : PagedQueryBase
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public bool Latest { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string State { get; set; }
    }
}