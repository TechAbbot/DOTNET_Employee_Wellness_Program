using EWPM.Progress.Api.Dtos;
using EWPM.Progress.Api.Interface;
using EWPM.Repository.Progress.Model;
using EWPM.Shared.Helper;
using EWPM.Shared.ViewModel;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/challenges/{challengeId}/progress")]
public class ProgressController : ControllerBase
{
    private readonly IProgressQueue _queue;

    public ProgressController(IProgressQueue queue)
    {
        _queue = queue;
    }
    /// <summary>
    /// Progress submit for user with challenge Id
    /// </summary>
    /// <param name="challengeId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Response> SubmitProgress(Guid challengeId, [FromBody] ProgressEntryDto dto)
    {
        var entry = new ProgressEntry
        {
            Id = Guid.NewGuid(),
            ChallengeId = challengeId,
            UserId = dto.UserId,
            Value = dto.Value,
            Timestamp = dto.Timestamp,
            EventId = Guid.NewGuid()
        };

        await _queue.QueueProgressAsync(entry);

        return new Response()
        {
            Data = "",
            Message = Constants.ProgressQueued,
            StatusCode = 200
        };
    }
}

