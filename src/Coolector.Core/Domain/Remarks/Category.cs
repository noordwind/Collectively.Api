namespace Coolector.Core.Domain.Remarks
{
    public class Category : Entity
    {
        public string Name { get; protected set; }

        protected Category(string name)
        {
            Name = name;
        }
    }
}