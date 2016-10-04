using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Services
{
    public interface IFileHandler
    {
        Task UploadAsync(File file, Action<string> onUploaded = null);
        Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(Guid remarkId);
        Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(string fileId);
    }
}