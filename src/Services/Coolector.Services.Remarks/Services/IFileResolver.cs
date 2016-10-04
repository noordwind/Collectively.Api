using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;

namespace Coolector.Services.Remarks.Services
{
    public interface IFileResolver
    {
        Maybe<File> FromBase64(string base64, string name, string contentType);
    }
}