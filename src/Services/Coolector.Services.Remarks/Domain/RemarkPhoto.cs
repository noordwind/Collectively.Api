using System;
using Coolector.Common.Extensions;
using Coolector.Services.Domain;

namespace Coolector.Services.Remarks.Domain
{
    public class RemarkPhoto : ValueObject<RemarkPhoto>
    {
        public string FileId { get; protected set; }

        protected RemarkPhoto()
        {
        }

        protected RemarkPhoto(string fileId)
        {
            if (fileId.Empty())
                throw new ArgumentException("Photo file id can not be empty.", nameof(fileId));

            FileId = fileId;
        }

        public static RemarkPhoto Empty => new RemarkPhoto();

        public static RemarkPhoto Create(string fileId)
            => new RemarkPhoto(fileId);

        protected override bool EqualsCore(RemarkPhoto other) => FileId.Equals(other.FileId);

        protected override int GetHashCodeCore() => FileId.GetHashCode();
    }
}