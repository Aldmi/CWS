using System.Text.RegularExpressions;

namespace Shared.Helpers
{
    public static class HelpersBool
    {
        public static bool ContainsHexSubStr(string matchString)
        {
            if (string.IsNullOrEmpty(matchString))
                return false;

            return Regex.IsMatch(matchString, @"0x[0-9a-fA-F]{2}");
        }
    }
}