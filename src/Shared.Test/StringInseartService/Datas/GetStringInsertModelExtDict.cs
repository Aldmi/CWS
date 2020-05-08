using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Types;

namespace Shared.Test.StringInseartService.Datas
{
    public static class GetStringInsertModelExtDict
    {
        public static IReadOnlyDictionary<string, StringInsertModelExt> SimpleDictionary => new ReadOnlyDictionary<string, StringInsertModelExt>(new Dictionary<string, StringInsertModelExt>
        {
            { "default", new StringInsertModelExt("default", string.Empty, null, null)},
            { "X1", new StringInsertModelExt("X1", ":X1", null, null)},
            { "X2", new StringInsertModelExt("X2", ":X2", null, null)},
            { "D3", new StringInsertModelExt("D3", ":D3", null, null)},
            { "t", new StringInsertModelExt("t", ":t", null, null)},
            { "X2_Border", new StringInsertModelExt("X2_Border", ":X2", new BorderSubString{StartCh = "0x02", EndCh = "0x03", IncludeBorder = true}, null)},
        });
    }
}