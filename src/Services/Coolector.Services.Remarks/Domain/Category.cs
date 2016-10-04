using System;
using Coolector.Common.Extensions;
using Coolector.Services.Domain;

namespace Coolector.Services.Remarks.Domain
{
    public class Category : IdentifiableEntity
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