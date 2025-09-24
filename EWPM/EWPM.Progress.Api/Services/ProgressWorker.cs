using EWPM.Progress.Api.Interface;
using EWPM.Repository.Progress.Data;
using EWPM.Repository.Progress.Model;
using StackExchange.Redis;

namespace EWPM.Progress.Api.Services
{
    public class ProgressWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProgressQueue _queue;
        private readonly IConnectionMultiplexer _redis;

        public ProgressWorker(IServiceProvider serviceProvider, IProgressQueue queue, IConnectionMultiplexer redis)
        {
            _serviceProvider = serviceProvider;
            _queue = queue;
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var buffer = new List<ProgressEntry>();
            var flushInterval = TimeSpan.FromSeconds(2);
            var lastFlush = DateTime.UtcNow;

            await foreach (var entry in _queue.DequeueAllAsync(stoppingToken))
            {
                buffer.Add(entry);

                var shouldFlush = buffer.Count >= 50 || (DateTime.UtcNow - lastFlush) >= flushInterval;
                if (shouldFlush)
                {
                    await ProcessBatchAsync(buffer, stoppingToken);
                    buffer.Clear();
                    lastFlush = DateTime.UtcNow;
                }
            }
        }

        private async Task ProcessBatchAsync(List<ProgressEntry> batch, CancellationToken token)
        {
            if (!batch.Any()) return;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ProgressDbContext>();

                // 1. Save to DB in bulk
                await dbContext.ProgressEntries.AddRangeAsync(batch, token);
                await dbContext.SaveChangesAsync(token);

                // 2. Update Redis for each entry
                var redisDb = _redis.GetDatabase();

                var grouped = batch
                    .GroupBy(e => e.ChallengeId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var kvp in grouped)
                {
                    string redisKey = $"leaderboard:{kvp.Key}";
                    foreach (var entry in kvp.Value)
                    {
                        // Increment the user's score instead of overwriting
                        await redisDb.SortedSetIncrementAsync(redisKey, entry.UserId, entry.Value);
                    }
                }

                Console.WriteLine($"[Worker] Processed batch of {batch.Count} entries.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Worker Error] {ex.Message}");
            }
        }
    }
}
