using System.Drawing;
using System.IO;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public class FileValidator : IFileValidator
    {
        public bool IsImage(File file)
        {
            try
            {
                using (var stream = new MemoryStream(file.Bytes))
                {
                    var bitmap = Image.FromStream(stream);

                    return !bitmap.Size.IsEmpty;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}