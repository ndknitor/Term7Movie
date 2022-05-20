using Newtonsoft.Json;
using Microsoft.AspNetCore.Session;
namespace Microsoft.AspNetCore.Mvc
{
    public static class ISessionExtention
    {
        public static void Set(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static object Get(this ISession session, string key)
        {
            return JsonConvert.DeserializeObject(session.GetString(key));
        }
        public static T Get<T>(this ISession session, string key)
        {
            return JsonConvert.DeserializeObject<T>(session.GetString(key));
        }
    }
}