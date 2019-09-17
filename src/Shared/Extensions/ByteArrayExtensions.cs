using System.Collections.Generic;
using System.Text;

namespace Shared.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ArrayByteToString(this IEnumerable<byte> source, string format)
        {
            format = (format == "HEX") ? "X2" : format;
            var stringBuilder = new StringBuilder();
            foreach (var item in source)
            {
                stringBuilder.Append(item.ToString(format));
            }
            return stringBuilder.ToString();
        }
    }
}