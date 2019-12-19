using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Factory
{
    public class AdInputTypeIndependentInseartsHandlersFactory : IIndependentInseartsHandlersFactory
    {
        public IIndependentInsertsHandler Create(StringInsertModel insertModel)
        {
            switch (insertModel.VarName)
            {
                case "TypeName":
                    return new TypeNameInseartHandler(insertModel);

                default: return null;
            }
        }
    }
}