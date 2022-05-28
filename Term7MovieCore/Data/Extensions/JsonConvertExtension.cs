using Newtonsoft.Json;

namespace Term7MovieCore.Extensions
{
    public static class JsonConvertExtension
    {
        public static string ToJson<T>(this T o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static T ToObject<T>(this string json)
        {
            return json != null ? JsonConvert.DeserializeObject<T>(json) : default;
        }
    }
}
