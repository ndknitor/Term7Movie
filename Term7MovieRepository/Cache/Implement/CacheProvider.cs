using StackExchange.Redis;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Extensions;
using Term7MovieRepository.Cache.Interface;

namespace Term7MovieRepository.Cache.Implement
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IDatabase redis;

        private static readonly TimeSpan DEFAULT_TIME_SPAN = TimeSpan.FromDays(1);
        public TimeSpan Expire { set; get; }
        public CacheProvider(IConnectionMultiplexer connectionMultiplexer)
        {
            redis = connectionMultiplexer.GetDatabase();
            Expire = DEFAULT_TIME_SPAN;
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            string value = await redis.StringGetAsync(key);

            return value != null ? value.ToObject<T>() : default;
        }

        public T GetValue<T>(string key)
        {
            string value = redis.StringGet(key);

            return value != null ? value.ToObject<T>() : default;
        }

        public async Task SetValueAsync (string key, object value)
        {
            await redis.StringSetAsync(key, value.ToJson(), Expire);
        }

        public void SetValue(string key, object value)
        {
            redis.StringSet(key, value.ToJson(), Expire);
        }
        
        public async Task RemoveAsync(string key)
        {
            await redis.KeyDeleteAsync(key);
        }

        public void Remove(string key)
        {
            redis.KeyDelete(key);
        }

        public bool IsExist(string key)
        {
            return redis.KeyExists(key);
        }

        public async Task<bool> IsExistAsync(string key)
        {
            return await redis.KeyExistsAsync(key);
        }

        public async Task PutHashMapAsync<T>(string key, IEnumerable<T> valueSet)
        {
            HashEntry[] entries = new HashEntry[valueSet.Count()];
            switch (valueSet.First())
            {
                case TicketDto:
                    TicketDto firstTicket = valueSet.First() as TicketDto;
                    string showtimeRedisKey = Constants.REDIS_KEY_SHOWTIME_TICKET + "_" + firstTicket.ShowTimeId;

                    TimeSpan expire = firstTicket.ShowStartTime - DateTime.UtcNow;
                    entries[0] = new HashEntry(firstTicket.ShowTimeId, showtimeRedisKey);
                    await redis.StringSetAsync(showtimeRedisKey, valueSet.ToJson(), expire);
                    break;
            }
            await redis.HashSetAsync(key, entries);
        }

        public async Task PutHashMapAsync(string key, HashEntry[] entries)
        {
            await redis.HashSetAsync(key, entries);
        }

        public async Task<bool> IsHashExistAsync(string hashKey, string key)
        {
            return await redis.HashExistsAsync(hashKey, key);
        }

        public bool IsHashExist(string hashKey, string key)
        {
            return redis.HashExists(hashKey, key);
        }

        public string GetHashFieldValue(string hashKey, string key)
        {
            object o = redis.HashGet(hashKey, key);

            return o == null ? default : o.ToString();
        }

        public async Task<string> GetHashFieldValueAsync(string hashKey, string key)
        {
            RedisValue o = await redis.HashGetAsync(hashKey, key);

            return o .IsNullOrEmpty? default : o.ToString();
        }
    }
}
