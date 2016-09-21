using System;
using Coolector.Core.Extensions;

namespace Coolector.Core.Domain.Remarks
{
    public class RemarkPhoto : IValueObject
    {
        public Guid Id { get; protected set; }
        public string InternalId { get; protected set; }
        public string Url { get; protected set; }

        protected RemarkPhoto()
        {
        }

        protected RemarkPhoto(Guid id, string internalId, string url)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Photo id can not be empty.", nameof(id));
            if (internalId.Empty())
                throw new ArgumentException("Photo name can not be empty.", nameof(internalId));
            if (url.Empty())
                throw new ArgumentException("Photo URL can not be empty.", nameof(url));

            Id = id;
            InternalId = internalId;
            Url = url;
        }

        public static RemarkPhoto Empty => new RemarkPhoto();

        public static RemarkPhoto Create(Photo photo)
            => new RemarkPhoto(photo.Id, photo.InternalId, photo.Url);
    }
}