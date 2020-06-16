using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Types;


namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public abstract class BaseInsH : IIndependentInsertsHandler
    {
        protected readonly StringInsertModel InsertModel;

        protected BaseInsH(StringInsertModel insertModel)
        {
            InsertModel = insertModel;
        }


        public Result<(Change<string>, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is AdInputType uit))
                return Result.Ok<ValueTuple<Change<string>, StringInsertModel>>((null, InsertModel));

            var lang = uit.Lang;
            var inseart = GetInseart(lang, uit);
            return Result.Ok((inseart, InsertModel));
        }

        protected abstract Change<string> GetInseart(Lang lang, AdInputType uit);
    }
}