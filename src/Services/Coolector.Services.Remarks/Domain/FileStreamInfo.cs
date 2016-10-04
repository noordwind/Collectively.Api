using System.IO;
using Coolector.Services.Domain;

namespace Coolector.Services.Remarks.Domain
{
    public class FileStreamInfo : ValueObject<FileStreamInfo>
    {
        public string Name { get; protected set; }
        public string ContentType { get; protected set; }
        public Stream Stream { get; protected set; }

        protected FileStreamInfo()
        {
        }

        protected FileStreamInfo(string name, string contentType, Stream stream)
        {
            Name = name;
            ContentType = contentType;
            Stream = stream;
        }

        public static FileStreamInfo Empty => new FileStreamInfo();

        public static FileStreamInfo Create(string name, string contentType, Stream stream)
            => new FileStreamInfo(name, contentType, stream);

        protected override bool EqualsCore(FileStreamInfo other) => Stream.Equals(other.Stream);

        protected override int GetHashCodeCore() => Stream.GetHashCode();
    }
}