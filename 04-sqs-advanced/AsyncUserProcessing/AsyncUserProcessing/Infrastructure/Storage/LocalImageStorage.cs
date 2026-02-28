using AsyncUserProcessing.Domain.Interfaces;

namespace AsyncUserProcessing.Infrastructure.Storage
{
    public class LocalImageStorage : IImageStorage
    {
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "original");

        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);

            var filePath = Path.Combine(_basePath, $"{Guid.NewGuid()}_{fileName}");

            using var file = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(file);

            return filePath;
        }
    }
}
