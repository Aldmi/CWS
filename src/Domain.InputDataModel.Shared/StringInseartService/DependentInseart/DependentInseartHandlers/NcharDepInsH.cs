using System;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Подсчет СИМВОЛОВ Nchar. Подстрока определяется границами.
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

        //TODO: переделать на работу по SubString
        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            //var nByteIndex = sbMutable.IndexOf(RequiredModel.Replacement, 0, false);
            //var crcIndex = sbMutable.IndexOf(_crcModel.Replacement, 0, false);
            //if(crcIndex == -1)
            //    return Result.Failure<string>($"Обработка NcharDepInsH не может быт выполненна. Не найдена замена в строке {_crcModel.Replacement}");

            //var startIndex = nByteIndex + RequiredModel.Replacement.Length;
            //var endIndex = crcIndex;
            //var delta = endIndex - startIndex;
            var lenght = borderedStr.Length;
            var resStr = RequiredModel.Ext.CalcFinishValue(lenght);
            return Result.Ok(resStr);
        }


        /// <summary>
        /// ТОЛЬКО BorderSubString ОПРЕДЕЛЯЕТ ПОДСТРОКУ ДЛЯ GetInseart
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            return Result.Failure<string>("Для NcharDepInsH подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
        }
    }
}