using System;
using System.Collections.Generic;
using Collectively.Common.Types;

namespace Collectively.Api.Queries
{
    public class BrowseRemarks : BrowseRemarksBase
    {
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string ResolverId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? AvailableGroupId { get; set; }
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