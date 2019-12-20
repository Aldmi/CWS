using System;
using CSharpFunctionalExtensions;

namespace Shared.Services.StringInseartService
{
    public interface IIndependentInsertsHandler
    {
        Result<ValueTuple<string, StringInsertModel>> CalcInserts(object inData);
    }
}