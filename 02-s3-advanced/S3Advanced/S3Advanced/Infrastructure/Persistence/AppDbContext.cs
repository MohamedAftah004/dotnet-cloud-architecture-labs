using Microsoft.EntityFrameworkCore;

namespace S3Advanced.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {
        }

        public DbSet<StoredFile> Files { set; get; }
    }
}
