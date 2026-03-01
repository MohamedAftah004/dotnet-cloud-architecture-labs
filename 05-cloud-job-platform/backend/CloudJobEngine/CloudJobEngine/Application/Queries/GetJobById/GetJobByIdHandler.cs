using CloudJobEngine.Application.DTOs;
using CloudJobEngine.Application.Interfaces;
using MediatR;

namespace CloudJobEngine.Application.Queries.GetJobById;

public class GetJobByIdHandler
    : IRequestHandler<GetJobByIdQuery, JobDto?>
{
    private readonly IJobRepository _jobRepository;

    public GetJobByIdHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobDto?> Handle(
        GetJobByIdQuery query,
        CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(query.JobId, cancellationToken);

        if (job is null)
            return null;

        return new JobDto
        {
            Id = job.Id,
            UserId = job.UserId,
            FileKey = job.FileKey,
            Status = job.Status.ToString(),
            CreatedAt = job.CreatedAt,
            ProcessedAt = job.ProcessedAt
        };
    }
}