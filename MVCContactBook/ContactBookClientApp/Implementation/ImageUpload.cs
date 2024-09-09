using ContactBookClientApp.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace ContactBookClientApp.Implementation
{

    [ExcludeFromCodeCoverage]
    public class ImageUpload : IImageUpload
    {
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ImageUpload(IWebHostEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }
        public string AddImageFileToPath(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

            // Save the file to storage and set path

            using (var stream = new FileStream(filePath, FileMode.Create))

            {

                imageFile.CopyTo(stream);

                return imageFile.FileName;

            }
        }

    }
}
