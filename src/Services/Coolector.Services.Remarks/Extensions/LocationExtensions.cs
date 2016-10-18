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
            var longitudeDifferenceInRadians = (target.Longitude - source.Longitude) * DistanceToRadians;
            var latitudeDifferenceInRadians = (target.Latitude - source.Latitude) * DistanceToRadians;
            var a = Math.Pow(Math.Sin(latitudeDifferenceInRadians / 2.0), 2.0) 
                + Math.Cos(source.Latitude * DistanceToRadians) 
                * Math.Cos(target.Latitude * DistanceToRadians) 
                * Math.Pow(Math.Sin(longitudeDifferenceInRadians / 2.0), 2.0);
            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            var distanceInKilometers = EarthRadius * c;

            return distanceInKilometers;
        }
    }
}