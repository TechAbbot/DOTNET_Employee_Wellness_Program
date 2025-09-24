using EWPM.Repository.Progress.Model;

namespace EWPM.Repository.Progress.Interface
{
    public interface IProgressRepository
    {
        Task<List<ProgressEntry>> GetByChallengeId(Guid challengeId);
        Task<List<ProgressEntry>> GetByUserId(Guid userId);
    }
}
