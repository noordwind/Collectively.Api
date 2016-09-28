using System;
using Coolector.Common.Extensions;


namespace Coolector.Core.Domain.Remarks
{
    public class RemarkCategory : IValueObject
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        protected RemarkCategory(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Category id can not be empty.", nameof(name));
            if (name.Empty())
                throw new ArgumentException("Category name can not be empty.", nameof(name));

            Id = id;
            Name = name;
        }

        public static RemarkCategory Create(Category category)
            => new RemarkCategory(category.Id, category.Name);
    }
}