using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Serilog;
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
        private const string ReplacePattern = @"\{\$[^{}:$]+\}";


        //TODO:передавать ReplacePattern в ctor. для этого нужно имунную регитстрация в Autofac. Т.е. в  ByRulesDataProvider будет внедрятся реализация именно для ByRulesDataProvider со своим ReplacePattern
        public InlineInseartService(InlineStringInsertModelStorage inlineStrInsStorage)
        {
            _factory = new InlineStringInsertModelFactory(inlineStrInsStorage);
        }


        public Result<string> ExecuteInseart(string baseStr)
        {
            return baseStr.ExecInlineInseart(ReplacePattern, _factory.GetInlineStr);
        }
    }
}