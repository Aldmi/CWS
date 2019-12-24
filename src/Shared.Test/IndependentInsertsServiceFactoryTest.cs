using System;
using System.Collections.Generic;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Factory;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory;
using Shared.Services.StringInseartService;

namespace Shared.Test
{
    public class IndependentInsertsServiceFactoryTest
    {
        const string Pattern = @"\{([^\{\}:]+)(:[^\}\{]+)\}";


        private List<Func<StringInsertModel, IIndependentInsertsHandler>> _handlerFactorys;
        public IndependentInsertsServiceFactoryTest()
        {
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory = new AdInputTypeIndependentInseartsHandlersFactory(); //TODO: попробовать внедрять через DI
            _handlerFactorys = new List<Func<StringInsertModel, IIndependentInsertsHandler>>
            {
                new BaseIndependentInseartsHandlersFactory().Create,
                inputTypeInseartsHandlersFactory.Create
            };
        }
    }
}