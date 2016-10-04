using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Dto.Common;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Coolector.Services.Storage.Files
{
    public class FileHandler : IFileHandler
    {
        private readonly IGridFSBucket _bucket;

        public FileHandler(IGridFSBucket bucket)
        {
            _bucket = bucket;
        }

        public async Task UploadAsync(string name, string contentType, Stream stream, Action<string> onUploaded = null)
        {
            var fileInBucketId = string.Empty;
            var metadata = new BsonDocument {{"contentType", contentType}};
            var fileId = await _bucket.UploadFromStreamAsync(name, stream, new GridFSUploadOptions
            {
                Metadata = metadata
            });
            fileInBucketId = fileId.ToString();
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