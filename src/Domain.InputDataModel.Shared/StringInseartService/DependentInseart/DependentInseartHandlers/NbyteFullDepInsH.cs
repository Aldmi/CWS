﻿using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Строка сначала преобразуется в массив байт по формату и возвращается длинна этого массива байт.
    /// ДЛЯ ВЫЧИСЛЕНИЯ ПОЛНОЙ ДЛИННЫ В БАЙТАХ ПРИНЯТЫ НЕКОТОРОЕ ДОПУЩЕНИЯ длин в байтах:
    /// длина адреса = 1
    /// длинна самого NbyteFull = 1
    /// длинна Crc = 1 
    /// </summary>
    public class NbyteFullDepInsH : BaseDepInsH
    {
        //Предустановленные значения размеров в байтах. (для другого протокола нужно сделать другую реализацию)
        private const int LenghtAddressInBytes = 1;
        private const int LenghtNByteInbytes = 1;
        private const int LenghtCrcInbytes = 1;
        

        public NbyteFullDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        /// <summary>
        /// </summary>
        /// <param name="borderedStr">строка для вычисления</param>
        /// <param name="format">формат преобразования строки к byte[]</param>
        /// <param name="sbMutable">NOT USE</param>
        /// <returns></returns>
        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            var buf = borderedStr.ConvertStringWithHexEscapeChars2ByteArray(format);
            var lenghtBody = buf.Length;
            var lenght = lenghtBody + LenghtAddressInBytes + LenghtNByteInbytes + LenghtCrcInbytes;
            var resStr = RequiredModel.Ext.CalcFinishValue(lenght);
            return Result.Ok(resStr);
        }


        /// <summary>
        /// ТОЛЬКО BorderSubString ОПРЕДЕЛЯЕТ ПОДСТРОКУ ДЛЯ GetInseart
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        { 
            return Result.Failure<string>("Для NbyteFullDepInsH подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
        }
    }
}