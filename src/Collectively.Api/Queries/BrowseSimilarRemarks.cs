using System.Collections.Generic;
using Collectively.Common.Types;

namespace Collectively.Api.Queries
{
    public class BrowseSimilarRemarks : BrowseRemarksBase
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}