using System;
using Coolector.Common.Extensions;
using Coolector.Services.Domain;

namespace Coolector.Services.Remarks.Domain
{
    public class RemarkAuthor : ValueObject<RemarkAuthor>
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }

        protected RemarkAuthor(string userId, string name)
        {
            if (userId.Empty())
                throw new ArgumentException("Author id can not be empty.", nameof(name));
            if (name.Empty())
                throw new ArgumentException("Author name can not be empty.", nameof(name));

            UserId = userId;
            Name = name;
        }

        public static RemarkAuthor Create(User user)
            => new RemarkAuthor(user.UserId, user.Name);

        protected override bool EqualsCore(RemarkAuthor other) => UserId.Equals(other.UserId);

        protected override int GetHashCodeCore() => UserId.GetHashCode();
    }
}