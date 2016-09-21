using System;
using Coolector.Core.Extensions;

namespace Coolector.Core.Domain.Remarks
{
    public class Photo : Entity, ITimestampable
    {
        public string FileName { get; protected set; }
        public string Url { get; protected set; }
        public long SizeBytes { get; protected set; }
        public string FileBucketId { get; protected set; }
        public DateTime CreatedAt { get; }

        protected Photo()
        {
        }

        public Photo(string fileName, string url, long sizeBytes)
        {
            if (fileName.Empty())
                throw new ArgumentException("File name can not be empty.", nameof(fileName));
            if (url.Empty())
                throw new ArgumentException("File URL can not be empty.", nameof(url));
            if (sizeBytes == 0)
                throw new ArgumentException("File size can not be 0.", nameof(url));

            FileName = fileName;
            Url = url;
            SizeBytes = sizeBytes;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetFileBucketId(string fileBucketId)
        {
            if (fileBucketId.Empty())
                throw new ArgumentException("File bucket id can not be empty.", nameof(fileBucketId));
            if (FileBucketId.EqualsCaseInvariant(fileBucketId))
                return;

            FileBucketId = fileBucketId;
        }
    }
}