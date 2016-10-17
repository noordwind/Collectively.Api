using Coolector.Services.Remarks.Domain;
using System;

namespace Coolector.Services.Remarks.Extensions
{
    public static class LocationExtensions
    {
        private const double DistanceToRadians = Math.PI / 180.0;
        private const double EarthRadius = 6378.1370;

        public static bool IsInRange(this Location source, Location target, double meters)
        {
            var distance = source.DistanceInKilometers(target)*1000;

            return distance <= meters;
        }

        public static double DistanceInKilometers(this Location source, Location target)
        {
            double dlong = (target.Longitude - source.Longitude) * DistanceToRadians;
            double dlat = (target.Latitude - source.Latitude) * DistanceToRadians;
            double a = Math.Pow(Math.Sin(dlat / 2.0), 2.0) 
                + Math.Cos(source.Latitude * DistanceToRadians) 
                * Math.Cos(target.Latitude * DistanceToRadians) 
                * Math.Pow(Math.Sin(dlong / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            double d = EarthRadius * c;

            return d;
        }
    }
}