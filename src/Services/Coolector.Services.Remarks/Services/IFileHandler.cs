using System;
using System.Threading.Tasks;
using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Services
{
    public interface IFileHandler
    {
        Task UploadAsync(File file, Action<string> onUploaded = null);
    }
}