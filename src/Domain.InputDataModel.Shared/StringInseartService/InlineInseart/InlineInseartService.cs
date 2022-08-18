using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.InlineInseart
{
    /// <summary>
    /// Сервис замены подстрок в строке.
    /// Соотвествие подстрок определяет InlineStringInsertModelStorage
    /// </summary>
    public class InlineInseartService
    {
        private readonly InlineStringInsertModelFactory _factory;
        private readonly string _replacePattern;


        public InlineInseartService(string replacePattern, InlineStringInsertModelStorage inlineStrInsStorage)
        {
            _replacePattern = replacePattern;
            _factory = new InlineStringInsertModelFactory(inlineStrInsStorage);
        }


        public Result<string> ExecuteInseart(string baseStr)
        {
            return baseStr.ExecInlineInseart(_replacePattern, _factory.GetInlineStr);
        }
    }
}