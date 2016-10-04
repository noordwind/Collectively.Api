using System;
using Coolector.Dto.Common;

namespace Coolector.Dto.Remarks
{
    public class RemarkDto
    {
        public Guid Id { get; set; }
        public RemarkAuthorDto Author { get; set; }
        public RemarkCategoryDto Category { get; set; }
        public LocationDto Location { get; set; }
        public FileDto Photo { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public RemarkAuthorDto Resolver { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public bool Resolved { get; set; }
    }
}