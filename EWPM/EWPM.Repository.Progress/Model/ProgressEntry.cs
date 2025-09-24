using System.ComponentModel.DataAnnotations.Schema;

namespace EWPM.Repository.Progress.Model
{
    [Table("ProgressEntries")]
    public class ProgressEntry
    {
        public Guid Id { get; set; }
        public Guid ChallengeId { get; set; }
        public string UserId { get; set; } = null!;
        public long Value { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid EventId { get; set; }
    }
}
