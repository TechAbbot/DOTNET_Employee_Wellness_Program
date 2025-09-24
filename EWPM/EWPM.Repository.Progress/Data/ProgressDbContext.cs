using EWPM.Repository.Progress.Model;
using Microsoft.EntityFrameworkCore;

namespace EWPM.Repository.Progress.Data
{
    public class ProgressDbContext : DbContext
    {
        public ProgressDbContext(DbContextOptions<ProgressDbContext> options) : base(options) { }
        public DbSet<ProgressEntry> ProgressEntries => Set<ProgressEntry>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProgressEntry>()
                .HasIndex(p => new { p.ChallengeId, p.Timestamp });
            builder.Entity<ProgressEntry>()
                .HasIndex(p => p.EventId).IsUnique(); // idempotency
        }
    }
}
