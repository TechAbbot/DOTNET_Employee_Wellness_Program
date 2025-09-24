using EWPM.Repository.Challenges.Model;
using EWPM.Repository.Challenges.Data;
using EWPM.Repository.Challenges.Interface;
using Microsoft.EntityFrameworkCore;

namespace EWPM.Repository.Challenges.Repository
{
    public class ChallengesRepository : IChallengesRepository
    {
        private readonly ChallengesDbContext _context;

        public ChallengesRepository(ChallengesDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Create challenge
        /// </summary>
        /// <param name="challenge"></param>
        /// <returns></returns>
        public async Task<ChallengModel> CreateChallenge(ChallengModel challenge)
        {
            await _context.Challenges.AddAsync(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }
        /// <summary>
        /// Get Challenge by Challenge Id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public async Task<ChallengModel> GetById(Guid challengeId) => await _context.Challenges.FirstOrDefaultAsync(x => x.Id == challengeId) ?? new ChallengModel();
    }
}
