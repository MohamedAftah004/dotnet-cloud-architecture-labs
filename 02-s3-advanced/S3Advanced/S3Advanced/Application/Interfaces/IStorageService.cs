public interface IStorageService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<Stream> GetFileAsync(string key);
}