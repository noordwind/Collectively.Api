namespace Coolector.Common.Commands.Remarks
{
    public class CreateRemark : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Base64File { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}