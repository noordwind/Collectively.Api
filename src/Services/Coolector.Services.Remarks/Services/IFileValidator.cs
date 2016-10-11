using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Services
{
    public interface IFileValidator
    {
        bool IsImage(File file);
    }
}