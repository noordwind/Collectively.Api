using System;

namespace Coolector.Core.Domain.Remarks
{
    public class RemarkPhoto : IValueObject
    {
        public Guid Id { get; protected set; }
        public string Url { get; protected set; }

        protected RemarkPhoto(Guid id, string url)
        {
            Id = id;
            Url = url;
        }

        public static RemarkPhoto Empty => new RemarkPhoto(Guid.Empty, string.Empty);
        public static RemarkPhoto Create(Photo photo) => new RemarkPhoto(photo.Id, photo.Url);
    }
}