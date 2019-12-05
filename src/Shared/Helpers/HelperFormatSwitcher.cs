using Shared.Extensions;

namespace Shared.Helpers
{
    public static class HelperFormatSwitcher
    {
        public static (string newStr, string newFormat) CheckSwitch2Hex(string str, string format)
        {
            string newStr;
            string newFormat;
            if (str.Contains("0x"))
            {
                var buf = str.ConvertStringWithHexEscapeChars2ByteArray(format);
                newStr = buf.ArrayByteToString("X2");
                newFormat = "HEX";
            }
            else
            {
                newStr = str;
                newFormat = format;
            }
            return (newStr: newStr, newFormat: newFormat);
        }
    }
}