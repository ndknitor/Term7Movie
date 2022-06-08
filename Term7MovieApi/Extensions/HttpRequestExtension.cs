using Term7MovieCore.Extensions;

namespace Term7MovieApi.Extensions
{
    public static class HttpRequestExtension
    {
        public static async Task<T> ToObjectAsync<T> (this HttpRequest request)
        {
            if (request.Body.CanRead)
            {

                request.EnableBuffering();

                T t = default;

                using(var reader = new StreamReader(request.Body, leaveOpen: true))
                {

                    string bodyText = await reader.ReadToEndAsync();

                    t = bodyText.ToObject<T>();

                    request.Body.Position = 0;
                }
                return t;
            }

            return default;
        }
    }
}
