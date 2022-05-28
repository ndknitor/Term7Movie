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
    }
}
