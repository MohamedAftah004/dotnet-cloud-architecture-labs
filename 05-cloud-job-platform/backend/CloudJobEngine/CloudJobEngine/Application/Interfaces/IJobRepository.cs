using CloudJobEngine.Domain.Entities;

namespace CloudJobEngine.Application.Interfaces;

public interface IJobRepository
{
    Task AddAsync(Job job, CancellationToken cancellationToken);
    Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}