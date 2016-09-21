using System;

namespace Coolector.Core.Domain.Remarks
{
    public class Position : IValueObject
    {
        public double Latitude { get; protected set; }
        public double Longitude { get; protected set; }

        protected Position(double latitude, double longitude)
        {
            if(latitude > 90 || latitude < -90)
                throw new ArgumentException($"Invalid latitude {latitude}", nameof(latitude));
            if (longitude > 180 || longitude < -180)
                throw new ArgumentException($"Invalid longitude {longitude}", nameof(longitude));

            Latitude = latitude;
            Longitude = longitude;
        }

        public static Position Zero => new Position(0, 0);

        public static Position Create(double latitude, double longitude)
            => new Position(latitude, longitude);
    }
}