using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.Helpers;

namespace Shared.CrcCalculate
{
    public  static class CrcHelper
    {
        public static (string startCh, string endCh, bool includeBorder) CalcBorderSubString(string crcOption)
        {
            if(string.IsNullOrEmpty(crcOption))
                return (null, null, false);
            
            var crcOptionInclude = Regex.Match(crcOption, "\\[(.*)\\]").Groups[1].Value;  //Xor[0x02-0x03]
            var crcOptionExclude = Regex.Match(crcOption, "\\((.*)\\)").Groups[1].Value;  //Xor(0x02-0x03)
            var includeBorder = !string.IsNullOrEmpty(crcOptionInclude);
            crcOption = includeBorder ? crcOptionInclude : crcOptionExclude;
            var startEndChars = crcOption.Split('-');
            var startChar = (startEndChars.Length >= 1) ? startEndChars[0] : String.Empty;
            var endChar = (startEndChars.Length >= 2) ? startEndChars[1] : String.Empty;
            return (startChar, endChar, includeBorder);
        }


        public static Result<byte[]> CalcCrc(StringBuilder sb, (string startCh, string endCh, bool includeBorder) border, string format, string replacement, Func<IReadOnlyList<byte>, byte[]> crc)
        {
            var (_, isFailure, value, error) = CalcCrcByteArray(sb, border, format, replacement);
            if (isFailure)
                return Result.Failure<byte[]>(error);
            
            var crcArray = crc(value);
            return Result.Ok(crcArray);
        }
        
        public static Result<byte[]> CalcCrcByteArray(StringBuilder sb, (string startCh, string endCh, bool includeBorder) border, string format, string replacement)
        {
            var str = sb.ToString();
            string matchString;
            var (startCh, endCh, includeBorder) = border;
            if (string.IsNullOrEmpty(startCh) && string.IsNullOrEmpty(endCh))
            {
                //Не заданны симолы начала и конца подсчета строки CRC. Берем от начала строки до CRC.
                matchString = Regex.Match(str, $"(.*){replacement}").Groups[1].Value;
                //УБРАТЬ МАРКЕРНЫЕ СИМОЛЫ ИЗ ПОДСЧЕТА CRC
                matchString = matchString.Replace("\u0002", string.Empty).Replace("\u0003", string.Empty);
            }
            else
            {
                //Оба симолы начала и конца заданы.
                var (_, isFailure, value, error) = StringBuilderExtensions.SubstringBetweenCharacters(str, startCh, endCh, includeBorder);
                if (isFailure)
                    return Result.Failure<byte[]>(error);
                
                matchString = value;
            }
            
            //ВЫЧИСЛИТЬ МАССИВ БАЙТ ДЛЯ ПОДСЧЕТА CRC
            // var crcBytes = HelpersBool.ContainsHexSubStr(matchString) ?
            //     matchString.ConvertStringWithHexEscapeChars2ByteArray(format) :
            //     matchString.ConvertString2ByteArray(format);

             var crcBytes = matchString.ConvertStringWithHexEscapeChars2ByteArray(format);
            return Result.Ok(crcBytes);//DEBUG
        }

        
    }
}