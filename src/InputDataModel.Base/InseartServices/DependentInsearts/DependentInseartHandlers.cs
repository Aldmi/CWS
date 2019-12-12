using System;
using System.Text;
using System.Text.RegularExpressions;
using Shared.CrcCalculate;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.DependentInsearts
{
    public static class DependentInseartHandlers
    {
        /// <summary>
        /// Подсчет кол-ва символов в кавычках следующих за маркером {NumberOfCharacters}
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="format">NOT USE</param>
        /// <returns></returns>
        public static string NumberOfCharactersInseartHandler(string str, string format)
        {
            if (str.Contains("}"))                                                           //если указанны переменные подстановки
            {
                var subStr = str.Split('}');
                StringBuilder resStr = new StringBuilder();
                for (var index = 0; index < subStr.Length; index++)
                {
                    var s = subStr[index];
                    var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                    //1. Подсчет кол-ва символов
                    if (replaseStr.Contains("NumberOfCharacters"))
                    {
                        var targetStr = (subStr.Length > (index + 1)) ? subStr[index + 1] : string.Empty;
                        if (Regex.Match(targetStr, "\\\"(.*)\"").Success) //
                        {
                            var matchString = Regex.Match(targetStr, "\\\"(.*)\\\"").Groups[1].Value;
                            if (!string.IsNullOrEmpty(matchString))
                            {
                                var lenght = matchString.TrimEnd('\\').Length;
                                var dateFormat = Regex.Match(replaseStr, "\\{NumberOfCharacters:(.*)\\}").Groups[1].Value;
                                var formatStr = !string.IsNullOrEmpty(dateFormat) ?
                                    string.Format(replaseStr.Replace("NumberOfCharacters", "0"), lenght.ToString(dateFormat)) :
                                    string.Format(replaseStr.Replace("NumberOfCharacters", "0"), lenght);
                                resStr.Append(formatStr);
                            }
                        }
                        continue;
                    }

                    //Добавим в неизменном виде спецификаторы байтовой информации.
                    resStr.Append(replaseStr);
                }
                return resStr.ToString().Replace("\\\"", string.Empty); //заменить \"
            }
            return str;
        }


        /// <summary>
        /// Подсчет длинны строки между маркерами {Nbyte} и {CRC}.
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="format">NOT USE</param>
        /// <returns></returns>
        public static string NByteInseartHandler(string str, string format)
        {
            var requestFillBodyWithoutConstantCharacters = str.Replace("STX", string.Empty).Replace("ETX", string.Empty);

            //ВЫЧИСЛЯЕМ NByte---------------------------------------------------------------------------
            int lenght = 0;
            string matchString = null;

            if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Success) //вычислили длинну строки между Nbyte и CRC
            {
                matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Groups[2].Value;
                lenght = matchString.Length;
            }
            else if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*)").Success)//вычислили длинну строки от Nbyte до конца строки
            {
                matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*)").Groups[1].Value;
                lenght = matchString.Length;
            }


            //ЗАПОНЯЕМ ВСЕ СЕКЦИИ ДО CRC
            var subStr = requestFillBodyWithoutConstantCharacters.Split('}');
            StringBuilder resStr = new StringBuilder();
            foreach (var s in subStr)
            {
                var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                if (replaseStr.Contains("Nbyte"))
                {
                    var formatStr = string.Format(replaseStr.Replace("Nbyte", "0"), lenght);
                    resStr.Append(formatStr);
                }
                else
                {
                    resStr.Append(replaseStr);
                }
            }
            return resStr.ToString();
        }


        /// <summary>
        /// Подсчет длинны ВСЕЙ строки. Если str содержит hex симолы 0x, то строка сначала преобразуется к HEX.
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="format">Кодировка строки для преобразования byte[]</param>
        /// <returns></returns>
        public static string NByteFullInseartHandler(string str, string format)
        {
            var requestFillBodyWithoutConstantCharacters = str.Replace("STX", string.Empty).Replace("ETX", string.Empty);

            //ВЫЧИСЛЯЕМ NByte---------------------------------------------------------------------------
            int lenght = 0;

            if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{NbyteFull(.*)}(.*){CRC(.*)}").Success)
            {
                var matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Groups[2].Value;
                if (HelpersBool.ContainsHexSubStr(matchString))
                {
                    var buf = matchString.ConvertStringWithHexEscapeChars2ByteArray(format);
                    var lenghtBody = buf.Length;
                    var lenghtAddress = 1;
                    var lenghtNByte = 1;
                    var lenghtCrc = 1;
                    lenght = lenghtBody + lenghtAddress + lenghtNByte + lenghtCrc;
                }
                else
                {
                    lenght = matchString.Length;
                }
            }

            //ЗАПОНЯЕМ ВСЕ СЕКЦИИ ДО CRC
            var subStr = requestFillBodyWithoutConstantCharacters.Split('}');
            StringBuilder resStr = new StringBuilder();
            foreach (var s in subStr)
            {
                var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                if (replaseStr.Contains("NbyteFull"))
                {
                    var formatStr = string.Format(replaseStr.Replace("NbyteFull", "0"), lenght);
                    resStr.Append(formatStr);
                }
                else
                {
                    resStr.Append(replaseStr);
                }
            }
            return resStr.ToString();
        }



        /// <summary>
        /// Подсчет CRC.
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="format">Кодировка строки для преобразования byte[]</param>
        /// <returns></returns>
        public static string CrcInseartHandler(string str, string format)
        {
            var crcType = Regex.Match(str, "{CRC(.*):(.*)}").Groups[1].Value;
            var crcOptionInclude = Regex.Match(crcType, "\\[(.*)\\]").Groups[1].Value;  //Xor[0x02-0x03]
            var crcOptionExclude = Regex.Match(crcType, "\\((.*)\\)").Groups[1].Value;  //Xor(0x02-0x03)
            var includeBorder = !string.IsNullOrEmpty(crcOptionInclude);
            var crcOption = includeBorder ? crcOptionInclude : crcOptionExclude;
            var startEndChars = crcOption.Split('-');
            var startChar = (startEndChars.Length >= 1) ? startEndChars[0] : String.Empty;
            var endChar = (startEndChars.Length >= 2) ? startEndChars[1] : String.Empty;
            
            string matchString;
            if (string.IsNullOrEmpty(startChar) && string.IsNullOrEmpty(endChar))
            {
                //Не заданны симолы начала и конца подсчета строки CRC. Берем от начала строки до CRC.
                matchString = Regex.Match(str, "(.*){CRC(.*)}").Groups[1].Value;
            }
            else
            {
                //Оба симолы начала и конца заданы.
                matchString = str.SubstringBetweenCharacters(startChar, endChar,includeBorder);
            }

            //УБРАТЬ МАРКЕРНЫЕ СИМОЛЫ ИЗ ПОДСЧЕТА CRC
            matchString = matchString.Replace("\u0002", string.Empty).Replace("\u0003", string.Empty);

            //ВЫЧИСЛИТЬ МАССИВ БАЙТ ДЛЯ ПОДСЧЕТА CRC
            var crcBytes = HelpersBool.ContainsHexSubStr(matchString) ?
                matchString.ConvertStringWithHexEscapeChars2ByteArray(format) :
                matchString.ConvertString2ByteArray(format);

            var replacement = $"CRC{crcType}";
            byte crc = 0x00;
            switch (crcType)
            {
                case string s when s.Contains("XorInverse"):
                    crc = CrcCalc.CalcXorInverse(crcBytes);
                    break;

                case string s when s.Contains("8Bit"):
                    crc = CrcCalc.Calc8Bit(crcBytes);
                    break;

                case string s when s.Contains("Xor"):
                    crc = CrcCalc.CalcXor(crcBytes);
                    break;

                case string s when s.Contains("Mod256"):
                    crc = CrcCalc.CalcMod256(crcBytes);
                    break;
            }
            str = string.Format(str.Replace(replacement, "0"), crc);
            return str;
        }



    }
}