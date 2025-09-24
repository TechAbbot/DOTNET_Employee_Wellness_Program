namespace EWPM.Challenges.Api.Dtos
{
    public class LeaderboardEntryDto
    {
        public string UserId { get; set; } = default!;
        public long TotalProgress { get; set; }
    }
}
