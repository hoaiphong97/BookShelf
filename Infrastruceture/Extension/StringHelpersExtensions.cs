using Infrastructure.Constant;
using System.Text.RegularExpressions;

namespace Infrastructure.Extension
{
    public static class StringHelpersExtensions
    {
        public static string UpperCaseFirstLetter(this string text)
        {
            return Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
        }

        public static string GetValueOrDefault(this string? value, string defaultValue)
        {
            return String.IsNullOrEmpty(value) ? defaultValue : value;
        }

        public static string GetStringDefaultOrFromEnvValue(this string? value, string evnName, string defaultValue = "")
        {
            string? envValue = Environment.GetEnvironmentVariable(evnName);
            return String.IsNullOrEmpty(value) ? (string.IsNullOrEmpty(envValue) ? defaultValue : envValue) : value;
        }

        public static List<string> GetListStringDefaultOrFromEnvValue(this List<string>? value, string evnName, string defaultValue = "", char splitChar = ',')
        {
            string? envValue = Environment.GetEnvironmentVariable(evnName);
            return value == null ? (string.IsNullOrEmpty(envValue) ? defaultValue.Split(splitChar).ToList() : envValue.Split(splitChar).ToList()) : value;
        }

        public static int ToInt(this string value, int _default = 0)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return _default;
            }
        }

        public static long ToLong(this string value, long _default = 0)
        {
            try
            {
                return long.Parse(value);
            }
            catch
            {
                return _default;
            }
        }

        public static bool DateTimeContain(this string? value, DateTime? input)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (input == null) return false;
            try
            {
                value.Replace("/", "-");
                value.Replace("\\", "-");
                var temp = input.Value.ToString("yyyy-MM-dd HH:mm:ss");
                return temp.Contains(value.Trim());
            }
            catch { return false; }
        }

        public static bool NumberContain(this string? value, decimal? input)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (input == null) return false;
            try
            {
                var temp = input.Value.ToString();
                return temp.Contains(value.Trim());
            }
            catch { return false; }
        }

        public static bool ContainsIgnoreCase(this string firstString, string secondString)
        {
            return firstString.Contains(secondString, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsToLower(this string firstString, string secondString)
        {
            return firstString.ToLower().Contains(secondString.ToLower());
        }

        public static DateTime? DateTimeFromUnixTimeSeconds(this string value)
        {
            try
            {
                long unixTimestamp = value.ToLong();
                if (unixTimestamp == 0)
                    return null;

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
                return dateTimeOffset.UtcDateTime;
            }
            catch
            {
                return null;
            }
        }

        public static string[] SplitAndRemoveEmpty(this string value, char separator = CoreConstant.SYSTEM_SEPARATOR_CHAR)
        {
            return value.SplitAndRemoveEmpty(new char[] { separator });
        }

        public static string[] SplitAndRemoveEmpty(this string value, char[] separators)
        {
            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool EqualsIgnoreCase(this string firstString, string secondString)
        {
            return firstString.Equals(secondString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string NormalizeUrl(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var urlNormalized = !url.EndsWith('/') ? url : url.TrimEnd('/');

                return urlNormalized;
            }
            return "";
        }

        public static string NormalizeWithDetailUrl(this string? url, string detail)
        {
            if ((!string.IsNullOrEmpty(url)) && (!string.IsNullOrEmpty(detail)))
            {
                var urlNormalized = !url.EndsWith('/') ? url : url.TrimEnd('/');
                var detailNormalized = !detail.StartsWith('/') ? detail : detail.TrimStart('/');
                detailNormalized = !detailNormalized.EndsWith('/') ? detailNormalized : detailNormalized.TrimEnd('/');

                return $"{urlNormalized}/{detailNormalized}";
            }
            return "";
        }

        public static List<string> ExtractVariables(this string formula)
        {
            // Use regular expressions to match variables
            var matches = Regex.Matches(formula, @"[A-Za-z]+");

            // Create a list to hold the variable names
            List<string> variableList = new List<string>();

            // Add matched variables to the list
            foreach (Match match in matches)
            {
                variableList.Add(match.Value);
            }

            return variableList;
        }
    }
}
