
using Microsoft.Extensions.Caching.Distributed;
using Term7MovieCore.Data;
using Term7MovieCore.Extensions;
using Term7MovieRepository.Cache.Interface;

namespace Term7MovieRepository.Cache.Implement
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;
        public DistributedCacheEntryOptions CacheOption { set; get; }

        public CacheProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

            CacheOption = new DistributedCacheEntryOptions();

            CacheOption.SetAbsoluteExpiration(DateTime.UtcNow.AddMinutes(int.MaxValue));

        }

        public CacheProvider(IDistributedCache distributedCache, DistributedCacheEntryOptions options) : this(distributedCache)
        {
            this.CacheOption = options;
        }   

        public async Task<T> GetValueAsync<T>(string key)
        {
            string value = await _distributedCache.GetStringAsync(key);

            return value != null ? value.ToObject<T>() : default;
        }

        public T GetValue<T>(string key)
        {
            string value = _distributedCache.GetString(key);

            return value != null ? value.ToObject<T>() : default;
        }

        public async Task SetValueAsync (string key, object value)
        {
            await _distributedCache.SetStringAsync(key, value.ToJson(), CacheOption);
        }

        public void SetValue(string key, object value) => _distributedCache.SetString(key, value.ToJson(), CacheOption);
        
        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public void Remove(string key) => _distributedCache.Remove(key);

    }
}
