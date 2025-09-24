using EWPM.Repository.Challenges.Model;
using Microsoft.EntityFrameworkCore;

namespace EWPM.Repository.Challenges.Data
{
    public class ChallengesDbContext : DbContext
    {
        public ChallengesDbContext(DbContextOptions<ChallengesDbContext> options) : base(options) { }
        public DbSet<ChallengModel> Challenges { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChallengModel>().HasKey(c => c.Id);
        }
    }
}
