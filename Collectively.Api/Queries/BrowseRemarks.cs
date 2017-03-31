using System.Collections.Generic;
using Collectively.Common.Types;

namespace Collectively.Api.Queries
{
    public class BrowseRemarks : PagedQueryBase
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string ResolverId { get; set; }
        public bool Latest { get; set; }
        public bool Disliked { get; set; }
        public bool OnlyLiked { get; set; }
        public bool OnlyDisliked { get; set; }
        public string UserFavorites { get; set; }
        public IEnumerable<string> States { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}