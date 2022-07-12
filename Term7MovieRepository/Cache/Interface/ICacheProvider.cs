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
        Task PutHashMapAsync<T>(string key, IEnumerable<T> valueSet);
        Task PutHashMapAsync(string key, HashEntry[] entries);
        bool IsExist(string key);
        Task<bool> IsExistAsync(string key);
        Task<bool> IsHashExistAsync(string hashKey, string key);
        bool IsHashExist(string hashKey, string key);
        string GetHashFieldValue(string hashKey, string key);
        Task<string> GetHashFieldValueAsync(string hashKey, string key);
        //public string[] GetAllHashKey(string hashKey);
        public RedisValue[] GetAllHashValue(string hashKey);
    }
}
