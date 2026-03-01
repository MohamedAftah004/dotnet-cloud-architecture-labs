using CloudJobEngine.Domain.Enums;

namespace CloudJobEngine.Application.DTOs;

public class JobResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public JobStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}