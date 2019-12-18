using System;
using CSharpFunctionalExtensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers
{
    public interface IIndependentInsertsHandler
    {
        Result<ValueTuple<string, StringInsertModel>> CalcInserts(object inData);
    }
}