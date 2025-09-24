using EWPM.Progress.Api.Interface;
using EWPM.Repository.Progress.Model;
using System.Threading.Channels;

namespace EWPM.Progress.Api.Services
{
    public class ProgressQueue : IProgressQueue
    {
        private readonly Channel<ProgressEntry> _channel;

        public ProgressQueue()
        {
            var options = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<ProgressEntry>(options);
        }

        public ValueTask QueueProgressAsync(ProgressEntry entry) =>
            _channel.Writer.WriteAsync(entry);

        public IAsyncEnumerable<ProgressEntry> DequeueAllAsync(CancellationToken cancellationToken) =>
            _channel.Reader.ReadAllAsync(cancellationToken);
    }
}