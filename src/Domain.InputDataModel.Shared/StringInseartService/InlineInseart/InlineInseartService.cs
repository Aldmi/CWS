using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Serilog;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.InlineInseart
{
    public class InlineInseartService
    {
        private readonly InlineStringInsertModelFactory _factory;
        private readonly ILogger _logger;
        private const string ReplacePattern = "!!!!";
        private const string IdentifyKeyPattern = "!!!!";


        public InlineInseartService(InlineStringInsertModelStorage inlineStrInsStorage, ILogger logger)
        {
            _factory = new InlineStringInsertModelFactory(inlineStrInsStorage);
            _logger = logger;
        }



        public Result<string> ExecuteInseart(string baseStr)
        {
            //TODO: возможно еще 1 паттерн передавать для извленения ключа из найденной строки по ReplacePattern {$Time}  нужно извлечь time
            var resStr = baseStr.ExecInlineInseart(ReplacePattern, IdentifyKeyPattern, _factory.GetInlineStr);
            return Result.Ok(baseStr);
        }

    }
}