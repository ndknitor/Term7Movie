using System.Security.Cryptography;
using System.Text;

namespace Term7MovieCore.Extensions
{
    public static class StringHashAlgorithm
    {
        public static string ToSHA256String(this string s)
        {
            byte[] bytes = null;
            using (var sha256 = SHA256.Create())
            {
                bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
            return BitConverter.ToString(bytes);
        }

        public static string ToBase64String(this string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(bytes);
        }

        public static string ToHS256String(this string s, string key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] textBytes = encoding.GetBytes(s);
            byte[] keyBytes = encoding.GetBytes(key);

            byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
            {
                hashBytes = hash.ComputeHash(textBytes);
            }

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
