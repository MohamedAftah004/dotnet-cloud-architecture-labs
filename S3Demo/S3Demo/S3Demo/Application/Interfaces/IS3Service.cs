namespace S3Demo.Services
{
    public interface IS3Service
    {
        Task UploadFileAsync(IFormFile file);
        Task<byte[]> GetFileAsync(string fileName);
    }
}
