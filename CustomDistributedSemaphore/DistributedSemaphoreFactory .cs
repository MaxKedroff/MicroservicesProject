using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDistributedSemaphore
{
    public class DistributedSemaphoreFactory : IDistributedSemaphoreFactory
    {
        private readonly IConnectionMultiplexer _redis;

        public DistributedSemaphoreFactory(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public IDistributedSemaphore CreateSemaphore(string name, int maxCount)
        {
            return new RedisDistributedSemaphore(_redis.GetDatabase(), name, maxCount);
        }
    }
}
