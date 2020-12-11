using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.OpcServer.Model;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Types;

namespace Domain.InputDataModel.OpcServer.ForProviderImpl.IndependentInseartsImpl.Handlers
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
            if (!(inData is OpcInputType uit))
                return Result.Ok<ValueTuple<Change<string>, StringInsertModel>>((null, InsertModel));

            var inseart = GetInseart(uit);
            return Result.Ok((inseart, InsertModel));
        }

        protected abstract Change<string> GetInseart(OpcInputType uit);
    }
}