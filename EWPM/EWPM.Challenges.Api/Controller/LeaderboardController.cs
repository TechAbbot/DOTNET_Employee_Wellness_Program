using EWPM.Challenges.Api.Dtos;
using EWPM.Shared.ViewModel;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace EWPM.Challenges.Api.Controller
{
    [ApiController]
    [Route("api/challenges/{challengeId}/leaderboard")]
    public class LeaderboardController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeaderboardController(IConnectionMultiplexer redis, IHttpClientFactory httpClientFactory)
        {
            _redis = redis;
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// Get Leaderboard data
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response> GetLeaderboard(Guid challengeId)
        {
            var db = _redis.GetDatabase();
            string redisKey = $"leaderboard:{challengeId}";

            //First Try Redis 
            var redisEntries = await db.SortedSetRangeByRankWithScoresAsync(
                redisKey,
                0, 9, // top 10
                Order.Descending);

            if (redisEntries.Length > 0)
            {
                var result = redisEntries.Select(x => new LeaderboardEntryDto
                {
                    UserId = x.Element.ToString(),
                    TotalProgress = (long)x.Score
                }).ToList();

                return new Response()
                {
                    Data = result,
                    Message = "",
                    StatusCode = 200
                };
            }
            var client = _httpClientFactory.CreateClient("Progress");
            //If empty build from DB
            var progress = await client.GetFromJsonAsync<List<ProgressSharedModel>>($"api/Map/GetByChallengeId?challengeId={challengeId}");
            var dbResult = progress
                .Where(p => p.ChallengeId == challengeId)
                .GroupBy(p => p.UserId)
                .Select(g => new LeaderboardEntryDto
                {
                    UserId = g.Key,
                    TotalProgress = g.Sum(x => x.Value)
                })
                .OrderByDescending(x => x.TotalProgress)
                .Take(10)
                .ToList() ?? new List<LeaderboardEntryDto>();

            // Store back into Redis for future
            if (dbResult.Any())
            {
                await db.ExecuteAsync("FLUSHDB");
                var entries = dbResult.Select(x => new SortedSetEntry(x.UserId, x.TotalProgress)).ToArray();
                await db.SortedSetAddAsync(redisKey, entries);
            }
            return new Response()
            {
                Data = dbResult,
                Message = "",
                StatusCode = 200
            };
        }
    }
}
