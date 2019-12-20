using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public abstract class BaseInseartHandler : IIndependentInsertsHandler
    {
        private readonly StringInsertModel _insertModel;

        protected BaseInseartHandler(StringInsertModel insertModel)
        {
            _insertModel = insertModel;
        }

        public Result<(string, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is AdInputType uit))
                return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));

            var lang = uit.Lang;
            var resStr= GetInseart(lang, uit);
            return Result.Ok((resStr, _insertModel));
        }

        protected abstract string GetInseart(Lang lang, AdInputType uit);
    }
}