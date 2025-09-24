using EWPM.Repository.Challenges.Interface;
using EWPM.Repository.Challenges.Model;
using EWPM.Shared.Helper;
using EWPM.Shared.ViewModel;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChallengesController : ControllerBase
{
    private readonly IChallengesRepository _challengeRepository;

    public ChallengesController(IChallengesRepository challengeRepository)
    {
        _challengeRepository = challengeRepository;
    }
    /// <summary>
    /// Create challenge
    /// </summary>
    /// <param name="challenge"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Response> CreateChallenge([FromBody] ChallengModel challenge)
    {
        var result = await _challengeRepository.CreateChallenge(challenge);
        return new Response()
        {
            Data = result,
            Message = Constants.ChallengeAdd,
            StatusCode = 200
        };
    }
}
