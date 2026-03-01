namespace CloudJobEngine.Application.DTOs;

public class PresignedUrlResponse
{
    public string UploadUrl { get; set; } = string.Empty;
    public string FileKey { get; set; } = string.Empty;
}