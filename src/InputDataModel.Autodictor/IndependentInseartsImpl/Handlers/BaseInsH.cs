using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;


namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public abstract class BaseInsH : IIndependentInsertsHandler
    {
        protected readonly StringInsertModel InsertModel;

        protected BaseInsH(StringInsertModel insertModel)
        {
            InsertModel = insertModel;
        }


        public Result<(string, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is AdInputType uit))
                return Result.Ok<ValueTuple<string, StringInsertModel>>((null, InsertModel));

            var lang = uit.Lang;
            var resStr= GetInseart(lang, uit);
            return Result.Ok((resStr, InsertModel));
        }

        protected abstract string GetInseart(Lang lang, AdInputType uit);
    }
}