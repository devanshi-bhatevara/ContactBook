using System.Diagnostics.CodeAnalysis;

namespace ContactBookClientApp.Infrastructure
{
    public interface IImageUpload
    {
        string AddImageFileToPath(IFormFile imageFile);

    }
}
