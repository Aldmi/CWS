using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers;

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
        public static string BitConverter2StrByFormat(this byte[] arr, string format, ByteHexDelemiter hexDelemiter = ByteHexDelemiter.None)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));

            string resStr = string.Empty;
            switch (arr.Length)
            {
                case 0:
                    return string.Empty;

                case 1:
                    var byteVal = arr[0];
                    resStr = byteVal.Convert2StrByFormat(format);
                    break;

                case 2:
                    var int16Val = BitConverter.ToInt16(arr);
                    resStr= int16Val.Convert2StrByFormat(format);
                    break;
                
                case 4:
                    var int32Val = BitConverter.ToInt32(arr);
                    resStr = int32Val.Convert2StrByFormat(format);
                    break;

                case 8:
                    var int64Val = BitConverter.ToInt64(arr);
                    resStr = int64Val.Convert2StrByFormat(format);
                    break;
            }

            //вставить разделитель байт в получившуюся строку.
            if (hexDelemiter == ByteHexDelemiter.Hex)
            {
                var inseartVal = "0x";
                for (var i = 0; i < resStr.Length; i += 2 + inseartVal.Length)
                {
                    resStr = resStr.Insert(i, inseartVal);
                }
            }
            return resStr;
        }
    }
}