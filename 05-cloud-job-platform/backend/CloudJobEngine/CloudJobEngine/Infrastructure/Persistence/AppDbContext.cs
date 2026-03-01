using CloudJobEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudJobEngine.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {

        public DbSet<Job> Jobs => Set<Job>();

        public AppDbContext(DbContextOptions options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

    }
}
