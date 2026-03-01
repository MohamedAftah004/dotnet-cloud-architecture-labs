namespace CloudJobEngine.Application.DTOs;

public class JobDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FileKey { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}