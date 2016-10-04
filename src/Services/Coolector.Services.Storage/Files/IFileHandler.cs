using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Files
{
    public interface IFileHandler
    {
        Task UploadAsync(string name, string contentType, Stream stream, Action<string> onUploaded = null);
        Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(Guid remarkId);
        Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(string fileId);
    }
}