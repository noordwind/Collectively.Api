using System;
using Coolector.Core.Domain.Users;

namespace Coolector.Core.Domain.Remarks
{
    public class RemarkAuthor : IValueObject
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        protected RemarkAuthor(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static RemarkAuthor Create(User user)
            => new RemarkAuthor(user.Id, user.Name);
    }
}