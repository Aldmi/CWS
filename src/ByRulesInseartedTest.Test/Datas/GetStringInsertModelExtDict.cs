using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace ByRulesInseartedTest.Test.Datas
{
    public static class GetStringInsertModelExtDict
    {
        public static IReadOnlyDictionary<string, StringInsertModelExt> SimpleDictionary => new ReadOnlyDictionary<string, StringInsertModelExt>(new Dictionary<string, StringInsertModelExt>
        {
            { "Stations", new StringInsertModelExt("Stations", ":X2", null, null)}
        });



    }
}