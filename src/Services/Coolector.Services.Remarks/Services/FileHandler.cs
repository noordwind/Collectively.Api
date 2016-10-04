using System;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly IGridFSBucket _bucket;

        public FileHandler(IGridFSBucket bucket)
        {
            _bucket = bucket;
        }

        public async Task UploadAsync(File file, Action<string> onUploaded = null)
        {
            var fileInBucketId = string.Empty;
            using (var stream = new MemoryStream(file.Bytes))
            {
                var metadata = new BsonDocument {{"contentType", file.ContentType}};
                var fileId = await _bucket.UploadFromStreamAsync(file.Name, stream, new GridFSUploadOptions
                {
                    Metadata = metadata
                });
                fileInBucketId = fileId.ToString();
            }
            onUploaded?.Invoke(fileInBucketId);
        }
    }
}