using System;
using Coolector.Services.Domain;

namespace Coolector.Services.Remarks.Domain
{
    public class Location : ValueObject<Location>
    {
        public string Address { get; protected set; }
        public Position Position { get; protected set; }

        protected Location(Position position, string address = null)
        {
            if (position == null)
                throw new ArgumentException("Position can not be null.", nameof(position));

            Position = position;
            Address = address;
        }

        public static Location Empty => new Location(Position.Zero);

        public static Location Create(Position position, string address = null)
            => new Location(position, address);

        public static Location Create(double latitude, double longitude, string address = null)
            => new Location(Position.Create(latitude, longitude), address);

        protected override bool EqualsCore(Location other)
            => Address.Equals(other.Address) && Position.Equals(other.Position);

        protected override int GetHashCodeCore()
        {
            return Position.GetHashCode();
        }
    }
}