using System;

namespace Coolector.Core.Domain.Remarks
{
    public class Location : IValueObject
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

        public static Location Create(double latitude, double longitude, string address = null)
            => new Location(Position.Create(latitude, longitude), address);
    }
}