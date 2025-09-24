using EWPM.Challenges.Api.Dtos;
using EWPM.Repository.Challenges.Interface;
using EWPM.Shared.Helper;
using EWPM.Shared.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EWPM.Challenges.Api.Controller
{
    [ApiController]
    [Route("api/users/{userId}/challenges")]
    public class UserChallengesController : ControllerBase
    {
        private readonly IChallengesRepository _challengeRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserChallengesController(IHttpClientFactory httpClientFactory, IChallengesRepository challengeRepository)
        {
            _challengeRepository = challengeRepository;
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// Get Active challenges via userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("active")]
        public async Task<Response> GetActiveChallenges(string userId)
        {
            // Find challenges where user has any progress
            var client = _httpClientFactory.CreateClient(Constants.Progress);
            var progress = await client.GetFromJsonAsync<List<ProgressSharedModel>>($"api/Map/GetByUserId?userId={userId}");
            var userChallengeIds = progress?
                .Where(p => p.UserId == userId)
                .Select(p => p.ChallengeId)
                .Distinct()
                .ToList() ?? new List<Guid>();

            if (!userChallengeIds.Any())
                return new Response()
                {
                    Data = new List<ActiveChallengeDto>(),
                    Message = Constants.NoDataForUser,
                    StatusCode = 200
                };

            // Call Challenges API to get details
            var activeChallenges = new List<ActiveChallengeDto>();

            foreach (var challengeId in userChallengeIds)
            {            
                var challenge = await _challengeRepository.GetById(challengeId);
                if (challenge != null && challenge.StartDate <= DateTime.UtcNow && challenge.EndDate >= DateTime.UtcNow)
                {
                    ActiveChallengeDto activeChallenge = new ActiveChallengeDto()
                    {
                        ChallengeId = challenge.Id,
                        EndDate = challenge.EndDate,
                        Goal = challenge.Goal,
                        Name = challenge.Name,
                        StartDate = challenge.StartDate
                    };
                    activeChallenges.Add(activeChallenge);
                }
            }

            return new Response()
            {
                Data = activeChallenges,
                Message = "",
                StatusCode = 200
            };
        }
    }
}
