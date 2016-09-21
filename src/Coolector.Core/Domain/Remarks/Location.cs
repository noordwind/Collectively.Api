namespace Coolector.Core.Domain.Remarks
{
    public class Location : IValueObject
    {
        public string Address { get; protected set; }
        public Position Position { get; protected set; }

        protected Location(string address, Position position)
        {
            Address = address;
            Position = position;
        }

        public static Location Empty => new Location(string.Empty, Position.Zero);

        public static Location Create(string address, double latitude, double longitude)
            => new Location(address, Position.Create(latitude, longitude));
    }
}