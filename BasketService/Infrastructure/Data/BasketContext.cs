using StackExchange.Redis;


namespace Infrastructure.Data
{
    public class BasketContext
    {
        private readonly IConnectionMultiplexer _redis;

        public BasketContext(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public IDatabase Database => _redis.GetDatabase();
    }
}
