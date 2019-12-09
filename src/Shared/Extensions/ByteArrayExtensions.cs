using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ArrayByteToString(this IEnumerable<byte> source, string format)
        {
            var buf = source.ToArray();
            format = (format == "HEX") ? "X2" : format;
            if (format == "X2")
            {
                var stringBuilder = new StringBuilder();
                foreach (var item in buf)
                {
                    stringBuilder.Append(item.ToString(format));
                }
                return stringBuilder.ToString();
            }
            string converted = Encoding.GetEncoding(format).GetString(buf, 0, buf.Length);
            return converted;
        }
    }
}