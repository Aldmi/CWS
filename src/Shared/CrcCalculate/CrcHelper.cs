using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.Helpers;
using Shared.Types;

namespace Shared.CrcCalculate
{
    public  static class CrcHelper
    {
        /// <summary>
        /// Вычисляет CRC.
        /// Выделяет подстроку из sb, ограниченную border -> Переводит в массив байт по format -> вычисляет CRC по алгоритму calcCrc.
        /// Переводит 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="calcBorder"></param>
        /// <param name="format">формат перевода string в hex</param>
        /// <param name="replacement">выражение для замены</param>
        /// <param name="calcCrc">Алгоритм вычисления СRC</param>
        /// <returns></returns>
        public static Result<byte[]> CalcCrc(StringBuilder sb, Func<string, string, Result<string>> calcBorder, string format, string replacement, Func<IReadOnlyList<byte>, byte[]> calcCrc)
        {
            var (_, isFailure, value, error) = CalcCrcByteArray(sb, calcBorder, format, replacement);
            if (isFailure)
                return Result.Failure<byte[]>(error);
            
            var crcArray = calcCrc(value);
            return Result.Ok(crcArray);
        }
        

        private static Result<byte[]> CalcCrcByteArray(StringBuilder sb, Func<string, string, Result<string>> calcBorder, string format, string replacement)
        {
            var str = sb.ToString();
            var(_, isFailure, value, error)= calcBorder(str, replacement);
            if (isFailure) 
                return Result.Failure<byte[]>(error);

            var res = value.Replace("\u0002", string.Empty).Replace("\u0003", string.Empty);
            var crcBytes = res.ConvertStringWithHexEscapeChars2ByteArray(format);
            return Result.Ok(crcBytes);
        }
    }
}