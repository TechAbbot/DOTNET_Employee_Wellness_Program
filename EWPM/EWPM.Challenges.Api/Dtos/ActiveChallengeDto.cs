namespace EWPM.Challenges.Api.Dtos
{
    public class ActiveChallengeDto
    {
        public Guid ChallengeId { get; set; }
        public string Name { get; set; } = default!;
        public string Goal { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
