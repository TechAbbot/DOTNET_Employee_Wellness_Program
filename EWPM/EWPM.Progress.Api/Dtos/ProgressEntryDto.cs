namespace EWPM.Progress.Api.Dtos
{
    public class ProgressEntryDto
    {
        public string UserId { get; set; } = default!;
        public long Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
