namespace ProniaFrontToBack.Utilities.ImageUpload
{
    public static class ImageExtension
    {
        public static string SaveImage(this IFormFile imageFile, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string fullPath = Path.Combine(path, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return fileName;
        }
        public static string DeleteImage(this string imageName, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            string fullPath = Path.Combine(path, imageName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return imageName;
        }
    }
}