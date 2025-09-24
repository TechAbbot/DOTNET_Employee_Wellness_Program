using EWPM.Repository.Progress.Interface;
using EWPM.Repository.Progress.Data;
using EWPM.Repository.Progress.Model;
using Microsoft.EntityFrameworkCore;

namespace EWPM.Repository.Progress.Repository
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly ProgressDbContext _context;
        public ProgressRepository(ProgressDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get All Progress entries via ChallengeId
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public async Task<List<ProgressEntry>> GetByChallengeId(Guid challengeId)
        {
            return await _context.ProgressEntries.Where(x => x.ChallengeId == challengeId).ToListAsync();
        }
        /// <summary>
        /// Get All Progress entries via UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ProgressEntry>> GetByUserId(Guid userId)
        {
            return await _context.ProgressEntries.Where(x => x.UserId == userId.ToString()).ToListAsync();
        }
    }
}
