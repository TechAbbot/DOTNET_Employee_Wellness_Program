using EWPM.Progress.Api.Interface;
using EWPM.Repository.Progress.Interface;
using EWPM.Shared.ViewModel;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Map")]
public class MapController : ControllerBase
{
    private readonly IProgressRepository _progressRepository;

    public MapController(IProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
    }
    /// <summary>
    /// Internal Use API communication
    /// </summary>
    /// <param name="challengeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetByChallengeId")]
    public async Task<List<ProgressSharedModel>> GetByChallengeId(Guid challengeId)
    {
        var data = await _progressRepository.GetByChallengeId(challengeId);
        List<ProgressSharedModel> sharedModels = new List<ProgressSharedModel>();
        data.ForEach(x => sharedModels.Add(new ProgressSharedModel() { Id = x.Id,ChallengeId = x.ChallengeId, EventId = x.EventId, Timestamp = x.Timestamp, UserId = x.UserId, Value = x.Value}));
        return sharedModels;
    }
    /// <summary>
    /// Internal Use API communication
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetByUserId")]
    public async Task<List<ProgressSharedModel>> GetByUserId(Guid userId)
    {
        var data = await _progressRepository.GetByUserId(userId);
        List<ProgressSharedModel> sharedModels = new List<ProgressSharedModel>();
        data.ForEach(x => sharedModels.Add(new ProgressSharedModel() { Id = x.Id,ChallengeId = x.ChallengeId, EventId = x.EventId, Timestamp = x.Timestamp, UserId = x.UserId, Value = x.Value}));
        return sharedModels;
    }
}

