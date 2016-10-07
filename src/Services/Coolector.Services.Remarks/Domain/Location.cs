using System;
using Coolector.Services.Domain;

namespace Coolector.Services.Remarks.Domain
{
    public class Location : ValueObject<Location>
    {
        public double Longitude => Coordinates[0];
        public double Latitude => Coordinates[1];
        public double[] Coordinates { get; protected set; }
        public string Type { get; protected set; }
        public string Address { get; protected set; }

        protected Location()
        {
        }

        protected Location(double latitude, double longitude, string address = null)
        {
            if (latitude > 90 || latitude < -90)
                throw new ArgumentException($"Invalid latitude {latitude}", nameof(latitude));
            if (longitude > 180 || longitude < -180)
                throw new ArgumentException($"Invalid longitude {longitude}", nameof(longitude));

            Type = "Point";
            Coordinates = new[] { longitude, latitude };
            Address = address;
        }

        public static Location Zero => new Location(0, 0);

        public static Location Create(double latitude, double longitude, string address = null)
            => new Location(latitude, longitude, address);

        protected override bool EqualsCore(Location other)
            => Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);

        protected override int GetHashCodeCore()
        {
            var hash = 13;
            hash = (hash * 7) + Latitude.GetHashCode();
            hash = (hash * 7) + Longitude.GetHashCode();

            return hash;
        }
    }
}