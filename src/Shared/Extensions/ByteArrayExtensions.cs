using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Extensions
{
    public static class ByteArrayExtensions
    {
        static ByteArrayExtensions()
        {
            //Добавить обработку кодировки Windows-1251
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }


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



        /// <summary>
        /// Преобразует массив байт к строке по целочисленному формату..
        /// </summary>
        public static string BitConverter2StrByFormat(this byte[] arr, string format)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));

            Int64 value = 0; //64 разрадное число. чтобы вместить 8 байтное число
            switch (arr.Length)
            {
                case 0:
                    return string.Empty;

                case 1:
                    value = arr[0];
                    break;

                case 2:
                    value = BitConverter.ToInt16(arr);
                    break;

                case 4:
                    value = BitConverter.ToInt32(arr);
                    break;

                case 8:
                    value = BitConverter.ToInt64(arr);
                    break;
            }
            return value.Convert2StrByFormat(format);
        }
    }
}