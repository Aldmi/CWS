using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers
{
    public interface IIndependentInsertsHandler
    {
        Result<ValueTuple<string, StringInsertModel>> CalcInserts(object inData);
    }
}