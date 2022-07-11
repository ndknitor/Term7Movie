namespace Term7MovieCore.Data.Extensions
{
    public static class StringExtension
    {
        public static string ToSnakeCase(this string s)
        {
            if (s == null) return null;

            string result = s.ToLower().Trim();

            return result.Replace(" ", "_");
        }
    }
}
