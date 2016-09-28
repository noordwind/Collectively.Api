using System;
using Coolector.Common.Extensions;

namespace Coolector.Core.Domain.Remarks
{
    public class Category : Entity
    {
        public string Name { get; protected set; }

        protected Category()
        {
        }

        public Category(string name)
        {
            SetName(name);
        }

        public void SetName(string name)
        {
            if (name.Empty())
                throw new ArgumentException("Category name can not be empty.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Category name is too long.", nameof(name));
            if (Name.EqualsCaseInvariant(name))
                return;

            Name = name.ToLowerInvariant();
        }
    }
}