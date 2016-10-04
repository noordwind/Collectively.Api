using System;
using Coolector.Services.Domain;
using Coolector.Common.Extensions;

namespace Coolector.Services.Remarks.Domain
{
    public class Remark : IdentifiableEntity, ITimestampable
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
            RemarkPhoto photo, string description = null)
        {
            SetAuthor(author);
            SetCategory(category);
            SetLocation(location);
            SetPhoto(photo);
            SetDescription(description);
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

        public void SetPhoto(RemarkPhoto photo)
        {
            if (photo == null)
            {
                Photo = RemarkPhoto.Empty;

                return;
            }
            Photo = photo;
        }

        public void Resolve(User resolver)
        {
            if (Resolved)
            {
                throw new InvalidOperationException($"Remark {Id} has been already resolved " +
                                                    $"by {Resolver.Name} at {ResolvedAt}.");
            }
            Resolver = RemarkAuthor.Create(resolver);
            ResolvedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (description.Empty())
            {
                Description = string.Empty;

                return;
            }
            if (description.Length > 500)
                throw new ArgumentException("Description is too long.", nameof(description));
            if (Description.EqualsCaseInvariant(description))
                return;

            Description = description;
        }
    }
}