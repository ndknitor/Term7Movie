using StackExchange.Redis;
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

        public async Task PutHashMapAsync(string key, IEnumerable<object> valueSet)
        {
            HashEntry[] entries = new HashEntry[valueSet.Count()];
            int i = 0;
            switch (valueSet.First())
            {
                case TicketDto:
                    TimeSpan expire = (valueSet.First() as TicketDto).ShowStartTime - DateTime.UtcNow;
                    foreach (var ticket in (TicketDto[]) valueSet)
                    {
                        entries[i] = new HashEntry(ticket.Id.ToString(), ticket.Id.ToString());
                        await redis.StringSetAsync(ticket.Id.ToString(), ticket.ToJson(), expire);
                        i++;
                    }
                    break;
            }
            await redis.HashSetAsync(key, entries);
        }

        public async Task PutHashMapAsync(string key, HashEntry[] entries)
        {
            await redis.HashSetAsync(key, entries);
        }
    }
}
