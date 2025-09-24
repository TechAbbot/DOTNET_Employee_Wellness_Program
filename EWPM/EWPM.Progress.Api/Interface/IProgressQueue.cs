using EWPM.Repository.Progress.Model;

namespace EWPM.Progress.Api.Interface
{
    public interface IProgressQueue
    {
        ValueTask QueueProgressAsync(ProgressEntry entry);
        IAsyncEnumerable<ProgressEntry> DequeueAllAsync(CancellationToken cancellationToken);
    }
}
