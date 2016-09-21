using System;
using Coolector.Core.Domain.Users;

namespace Coolector.Core.Domain.Remarks
{
    public class Remark : Entity, ITimestampable
    {
        public RemarkAuthor Author { get; protected set; }
        public RemarkCategory Category { get; protected set; }
        public Location Location { get; protected set; }
        public RemarkPhoto Photo { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public RemarkAuthor Resolver { get; protected set; }
        public DateTime? ResolvedAt { get; protected set; }
        public bool Resolved => Resolver != null;

        protected Remark()
        {
        }

        public Remark(User author, Category category, Location location,
            Photo photo = null)
        {
            SetAuthor(author);
            SetCategory(category);
            SetLocation(location);
            SetPhoto(photo);
            CreatedAt = DateTime.UtcNow;
        }

        public void SetAuthor(User author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author), "Remark author can not be null.");

            Author = RemarkAuthor.Create(author);
        }

        public void SetCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Remark category can not be null.");

            Category = RemarkCategory.Create(category);
        }

        public void SetLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location), "Remark location can not be null.");

            Location = location;
        }

        public void SetPhoto(Photo photo)
        {
            if (photo == null)
            {
                Photo = RemarkPhoto.Empty;

                return;
            }
            Photo = RemarkPhoto.Create(photo);
        }

        public void Resolve(User resolver)
        {
            if (Resolved)
            {
                throw new DomainException($"Remark {Id} has been already resolved " +
                                          $"by {Resolver.Name} at {ResolvedAt}.");
            }
            Resolver = RemarkAuthor.Create(resolver);
            ResolvedAt = DateTime.UtcNow;
        }
    }
}