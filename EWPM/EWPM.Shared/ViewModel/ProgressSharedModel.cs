namespace EWPM.Shared.ViewModel
{
    public class ProgressSharedModel
    {
        public Guid Id { get; set; }
        public Guid ChallengeId { get; set; }
        public string UserId { get; set; } = null!;
        public long Value { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid EventId { get; set; }
    }
}
