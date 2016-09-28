using System;
using Coolector.Common.Extensions;

namespace Coolector.Core.Domain.Remarks
{
    public class RemarkAuthor : IValueObject
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        protected RemarkAuthor(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Author id can not be empty.", nameof(name));
            if (name.Empty())
                throw new ArgumentException("Author name can not be empty.", nameof(name));

            Id = id;
            Name = name;
        }

        public static RemarkAuthor Create(User user)
            => new RemarkAuthor(user.Id, user.Name);
    }
}