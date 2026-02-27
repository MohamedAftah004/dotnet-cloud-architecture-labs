using Microsoft.EntityFrameworkCore;
using S3Advanced.Infrastructure.Persistence;

namespace S3Advanced.Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StoredFile file)
        {
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }
        public async Task<StoredFile?> GetByIdAsync(Guid id)
        {
            return await _context.Files
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
