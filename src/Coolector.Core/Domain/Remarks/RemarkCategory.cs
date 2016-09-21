using System;

namespace Coolector.Core.Domain.Remarks
{
    public class RemarkCategory : IValueObject
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        protected RemarkCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static RemarkCategory Create(Category category)
            => new RemarkCategory(category.Id, category.Name);
    }
}