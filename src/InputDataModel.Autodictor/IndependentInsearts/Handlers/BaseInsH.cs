using System;
using System.Linq;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.MiddleWares;
using Shared.MiddleWares.Handlers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public abstract class BaseInsH : IIndependentInsertsHandler
    {
        protected readonly StringInsertModel InsertModel;
        private readonly StringHandlerMiddleWare _stringHandlerMiddle;


        protected BaseInsH(StringInsertModel insertModel)
        {
            InsertModel = insertModel;
            if (InsertModel.Options != null && InsertModel.Options.Count > 0)
            {
                var handlerOptions = InsertModel.Options[0];
                _stringHandlerMiddle = StringHandlerMiddleWareFactoryFromStrOptions.Create(handlerOptions);
            }
        }


        public Result<(string, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is AdInputType uit))
                return Result.Ok<ValueTuple<string, StringInsertModel>>((null, InsertModel));

            var lang = uit.Lang;
            var resStr= GetInseart(lang, uit);
            resStr = _stringHandlerMiddle == null ? resStr : _stringHandlerMiddle.Convert(resStr, 0);
            return Result.Ok((resStr, InsertModel));
        }

        protected abstract string GetInseart(Lang lang, AdInputType uit);
    }
}