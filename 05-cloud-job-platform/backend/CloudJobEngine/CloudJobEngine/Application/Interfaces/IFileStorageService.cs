namespace CloudJobEngine.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> GeneratePresignedUploadUrlAsync(
        string fileKey,
        CancellationToken cancellationToken);
}