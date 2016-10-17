namespace Coolector.Common.Events.Remarks.Models
{
    public class RemarkFile
    {
        public string InternalId { get; }
        public byte[] Bytes { get; }
        public string Name { get; }
        public string ContentType { get; }

        protected RemarkFile()
        {
        }

        public RemarkFile(string internalId, byte[] bytes,
            string name, string contentType)
        {
            InternalId = internalId;
            Bytes = bytes;
            Name = name;
            ContentType = contentType;
        }
    }
}