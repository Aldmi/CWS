using System;
using System.Linq;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.MiddleWares;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public abstract class BaseInsH : IIndependentInsertsHandler
    {
        protected readonly StringInsertModel InsertModel;
        private readonly StringHandlerMiddleWareWrapper _stringHandlerMiddleWareWrapper;


        protected BaseInsH(StringInsertModel insertModel)
        {
            InsertModel = insertModel;
            if (InsertModel.Options != null && InsertModel.Options.Any())
            {
                _stringHandlerMiddleWareWrapper = new StringHandlerMiddleWareWrapper(InsertModel.Options);
            }
        }


        public Result<(string, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is AdInputType uit))
                return Result.Ok<ValueTuple<string, StringInsertModel>>((null, InsertModel));

            var lang = uit.Lang;
            var resStr= GetInseart(lang, uit);
            resStr = _stringHandlerMiddleWareWrapper == null ? resStr : _stringHandlerMiddleWareWrapper.Convert(resStr);
            return Result.Ok((resStr, InsertModel));
        }

        protected abstract string GetInseart(Lang lang, AdInputType uit);
    }
}