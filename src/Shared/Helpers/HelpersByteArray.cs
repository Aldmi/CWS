using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;



namespace Shared.Helpers
{
    public static class HelpersByteArray
    {
        static HelpersByteArray()
        {
            //Добавить обработку кодировки Windows-1251
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }


        public static byte[] ConvertString2ByteArray(this string str, string format)
        {
            byte[] resultBuffer = null;
            switch (format)
            {
                //Распарсить строку в масив байт как она есть. 0203АА96 ...
                case "HEX":
                    resultBuffer = str.Split(2).Select(s => byte.Parse(s, NumberStyles.AllowHexSpecifier)).ToArray();
                    break;

                default:
                    resultBuffer = Encoding.GetEncoding(format).GetBytes(str).ToArray();
                    break;
        
            }
            return resultBuffer;
        }

        /// <summary>
        /// Конвертор строки в массив байт, для строк в которых содержится
        /// hex escape последовательность вида 0xYY -> YY
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static List<byte> ConvertStringWithHexEscapeChars2ByteArray(this string str, string format)
        {
            //УДАЛИТЬ "0x" С КОНЦА СТРОКИ
            if (str.EndsWith("0x"))
            {
                str = str.Remove(str.Length - 2, 2);
            }

            var resBufer = new List<byte>();
            var buffChars = str.ToCharArray();
            for (int i = 0; i < buffChars.Length; i++)
            {
                var ch = buffChars[i];
                //ДОБАВИТЬ HEX ЗАЧЕНИЕ КАК ЕСТЬ (0xYY => YY)
                if ((i + 3) < buffChars.Length)
                {
                    if (ch == '0' && buffChars[i + 1] == 'x')
                    {
                        var ch1 = buffChars[i + 2];
                        var ch2 = buffChars[i + 3];
                        var sumCh = new string(new[] { ch1, ch2 });
                        if (byte.TryParse(sumCh, NumberStyles.HexNumber, null, out var resByte))
                        {
                            resBufer.Add(resByte);
                            i += 3;
                        }
                        continue;
                    }
                }
                //ПРЕОБРАЗОВАТЬ В МАССИВ БАЙТ ПО ФОРМАТУ ОСТАЛЬНЫЕ СИМОЛЫ
                var encodedChars = Encoding.GetEncoding(format).GetBytes(new[] { ch });
                resBufer.AddRange(encodedChars);
            }
            return resBufer;
        }
    }
}