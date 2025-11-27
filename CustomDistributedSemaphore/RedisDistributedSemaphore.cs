using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDistributedSemaphore
{
    public class RedisDistributedSemaphore : IDistributedSemaphore
    {
        private readonly IDatabase _redisDb;
        private readonly string _semaphoreKey;
        private readonly string _channelName;
        private readonly int _maxCount;


        public int CurrentCount => (int)_redisDb.ListLength(_semaphoreKey);

        public string Name {get;}

        public RedisDistributedSemaphore(IDatabase redisDb, string name, int maxCount)
        {
            _redisDb = redisDb;
            _semaphoreKey = $"semaphore:{name}";
            _channelName = $"semaphore-channel:{name}";
            _maxCount = maxCount;
            Name = name;
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            var script = @"
                if redis.call('exists', KEYS[1]) == 0 then
                    for i=1,ARGV[1] do
                        redis.call('rpush', KEYS[1], '1')
                    end
                    return true
                end
                return false";
            await _redisDb.ScriptEvaluateAsync(
                script,
                new RedisKey[] { _semaphoreKey },
                new RedisValue[] { _maxCount });
        }

        public async Task AcquireAsync(TimeSpan? timeout = null)
        {
            timeout ??= TimeSpan.FromSeconds(30);
            var startTime = DateTime.UtcNow;

            while (DateTime.UtcNow - startTime < timeout.Value)
            {
                if (await TryAcquireAsync())
                    return;

                var tcs = new TaskCompletionSource<bool>();
                var subscriber = _redisDb.Multiplexer.GetSubscriber();

                await subscriber.SubscribeAsync(_channelName, (channel, value) =>
                {
                    tcs.TrySetResult(true);
                });

                var delayTask = Task.Delay(TimeSpan.FromMilliseconds(500));
                var completedTask = await Task.WhenAny(tcs.Task, delayTask);

                await subscriber.UnsubscribeAsync(_channelName);

                if (completedTask == delayTask)
                {
                    continue;
                }
            }

            throw new TimeoutException($"Не удалось захватить семафор '{Name}' за {timeout.Value.TotalSeconds} секунд");
        }

        public async Task ReleaseAsync()
        {
            await _redisDb.ListRightPushAsync(_semaphoreKey, "1");

            var subscriber = _redisDb.Multiplexer.GetSubscriber();
            await subscriber.PublishAsync(_channelName, "release");
        }

        public async Task<bool> TryAcquireAsync()
        {
            var result = await _redisDb.ListLeftPopAsync(_semaphoreKey);
            return result != RedisValue.Null;
        }
    }
}
