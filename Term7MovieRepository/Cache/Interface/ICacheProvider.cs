using StackExchange.Redis;

namespace Term7MovieRepository.Cache.Interface
{
    public interface ICacheProvider
    {
        TimeSpan Expire { set; get; }
        Task<T> GetValueAsync<T>(string key);
        T GetValue<T>(string key);
        Task SetValueAsync(string key, object value);
        void SetValue(string key, object value);
        Task RemoveAsync(string key);
        void Remove(string key);
        Task PutHashMapAsync(string key, IEnumerable<object> valueSet);
        Task PutHashMapAsync(string key, HashEntry[] entries);
    }
}
