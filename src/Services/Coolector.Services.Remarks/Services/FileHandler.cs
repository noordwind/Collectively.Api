using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Common.Extensions;
using Coolector.Services.Remarks.Domain;
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

        public async Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(string fileId)
        {
            if(fileId.Empty())
                return new Maybe<FileStreamInfo>();

            var fileFromBucket = await _bucket.OpenDownloadStreamAsync(new ObjectId(fileId));
            if(fileFromBucket == null || fileFromBucket.Length == 0)
                return new Maybe<FileStreamInfo>();

            return FileStreamInfo.Create(fileFromBucket.FileInfo.Filename,
                fileFromBucket.FileInfo.Metadata["contentType"].ToString(), fileFromBucket);
        }
    }
}