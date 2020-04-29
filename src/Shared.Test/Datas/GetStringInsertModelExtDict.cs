using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Shared.Test.Datas
{
    public static class GetStringInsertModelExtDict
    {
        public static IReadOnlyDictionary<string, StringInsertModelExt> SimpleDictionary => new ReadOnlyDictionary<string, StringInsertModelExt>(new Dictionary<string, StringInsertModelExt>
        {
            { "Stations", new StringInsertModelExt("Stations", ":X2", null, null)}
        });



    }
}