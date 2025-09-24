using System.ComponentModel.DataAnnotations.Schema;

namespace EWPM.Repository.Challenges.Model
{
    [Table("Challenge")]
    public class ChallengModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Goal { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
