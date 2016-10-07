using System;
namespace Coolector.Common.Commands.Remarks
{
    public class CreateRemark : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }
        public RemarkFile Photo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public class RemarkFile
        {
            public string Base64 { get; set; }
            public string Name { get; set; }
            public string ContentType { get; set; }
        } 
    }
}