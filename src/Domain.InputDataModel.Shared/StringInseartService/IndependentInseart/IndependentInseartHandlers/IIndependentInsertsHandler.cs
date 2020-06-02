using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Collections;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers
{
    public interface IIndependentInsertsHandler
    {
        Result<(Change<string>, StringInsertModel)> CalcInserts(object inData);
    }
}