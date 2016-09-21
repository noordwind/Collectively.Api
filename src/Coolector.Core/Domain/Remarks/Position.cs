namespace Coolector.Core.Domain.Remarks
{
    public class Position : IValueObject
    {
        public double Latitude { get; protected set; }
        public double Longitude { get; protected set; }

        protected Position(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static Position Zero => new Position(0, 0);

        public static Position Create(double latitude, double longitude)
            => new Position(latitude, longitude);
    }
}