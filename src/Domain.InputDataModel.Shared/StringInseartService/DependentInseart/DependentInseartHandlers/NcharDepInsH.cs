using System;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Подсчет СИМВОЛОВ между маркером Nbyte и CRC
    /// </summary>
    public class NcharDepInsH : BaseDepInsH
    {
        private readonly StringInsertModel _crcModel;
        public NcharDepInsH(StringInsertModel requiredModel, StringInsertModel crcModel) : base(requiredModel)
        {
            _crcModel = crcModel;
            if(_crcModel == null)
                throw new ArgumentNullException(nameof(crcModel));
        }
        
        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var nByteIndex = sb.IndexOf(RequiredModel.Replacement, 0, false);
            var crcIndex = sb.IndexOf(_crcModel.Replacement, 0, false);
            if(crcIndex == -1)
                return Result.Failure<string>($"Обработка NcharDepInsH не может быт выполненна. Не найдена замена в строке {_crcModel.Replacement}");

            var startIndex = nByteIndex + RequiredModel.Replacement.Length;
            var endIndex = crcIndex;
            var delta = endIndex - startIndex;
            var resStr = RequiredModel.Ext.CalcFinishValue(delta);
            return Result.Ok(resStr);
        }
    }
}