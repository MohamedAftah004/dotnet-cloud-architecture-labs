using CloudJobEngine.Application.Interfaces;
using CloudJobEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudJobEngine.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Job job, CancellationToken cancellationToken)
        {
            await _context.Jobs.AddAsync(job, cancellationToken);
        }

        public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }
    }
}
