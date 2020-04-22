using System;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
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
        
        private readonly StringInsertModel _crcModel;
        public NbyteFullDepInsH(StringInsertModel requiredModel, StringInsertModel crcModel) : base(requiredModel)
        {
            _crcModel = crcModel;
            if(_crcModel == null)
                throw new ArgumentNullException(nameof(crcModel));
        }

        
        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var str = sb.ToString();
            var res= StringInsertModel.CalcSubStringBeetween2Models(str, RequiredModel, _crcModel);
            if (res.IsFailure)
                return res;

            var subStr = res.Value;
            var buf = subStr.ConvertStringWithHexEscapeChars2ByteArray(format);
            var lenghtBody = buf.Length;
            var lenght = lenghtBody + LenghtAddressInBytes + LenghtNByteInbytes + LenghtCrcInbytes;
            var resStr = lenght.Convert2StrByFormat(RequiredModel.Format);
            return Result.Ok(resStr);
        }
        
    }
}