using System;
using Coolector.Core.Domain.Users;

namespace Coolector.Core.Domain.Remarks
{
    public class Remark : Entity, ITimestampable
    {
        public RemarkAuthor Author { get; protected set; }
        public RemarkCategory Category { get; protected set; }
        public Location Location { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ResolvedAt { get; protected set; }

        protected Remark()
        {
        }

        public Remark(User author, Category category, Location location)
        {
            Author = RemarkAuthor.Create(author);
            Category = RemarkCategory.Create(category);
            Location = location;
            CreatedAt = DateTime.UtcNow;
        }

        public void Resolve()
        {
            if (ResolvedAt.HasValue)
                throw new DomainException($"Remark {Id} has been already resolved at {ResolvedAt}");

            ResolvedAt = DateTime.UtcNow;
        }
    }
}