using EWPM.Repository.Challenges.Model;

namespace EWPM.Repository.Challenges.Interface
{
    public interface IChallengesRepository
    {
        Task<ChallengModel> CreateChallenge(ChallengModel challenge);
        Task<ChallengModel> GetById(Guid challengeId);
    }
}
