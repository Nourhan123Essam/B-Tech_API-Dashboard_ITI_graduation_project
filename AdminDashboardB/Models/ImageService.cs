using ApplicationB.Contracts_B.General;

namespace AdminDashboardB.Models
{
    public class ImageService:IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName = "ImageUrls")
        {
            if (imageFile == null)
                return "/default/default.jpg"; // Default image if no image is provided

            // Define the path to save the image
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            // Save the image to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return the relative path for storing in the database
            return Path.Combine("/ImageUrls", uniqueFileName).Replace("\\", "/");
        }
    }
}
