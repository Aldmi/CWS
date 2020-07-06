using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.HandlersOption;
using Shared.Types;

namespace ByRulesInseartedTest.Test.Datas
{
    public static class GetStringInsertModelExtDict
    {
        public static IReadOnlyDictionary<string, StringInsertModelExt> SimpleDictionary => new ReadOnlyDictionary<string, StringInsertModelExt>(new Dictionary<string, StringInsertModelExt>
        {
            { "default", new StringInsertModelExt("default", string.Empty, null, null)},
            { "X1", new StringInsertModelExt("X1", ":X1", null, null)},
            { "X2", new StringInsertModelExt("X2", ":X2", null, null)},
            { "X3", new StringInsertModelExt("X3", ":X3", null, null)},
            { "D3", new StringInsertModelExt("D3", ":D3", null, null)},
            { "D2", new StringInsertModelExt("D2", ":D2", null, null)},
            { "t", new StringInsertModelExt("t", ":t", null, null)},

            { "D2_BorderRightBeforeCrc", new StringInsertModelExt("D2_BorderRightBeforeCrc", ":D2", new BorderSubString{DelimiterSign = DelimiterSign.Right, EndCh = "{CRC", EndInclude = false}, null)},
            { "X2_BorderRightBeforeCrc", new StringInsertModelExt("X2_BorderRightBeforeCrc", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right, EndCh = "{CRC", EndInclude = false}, null)},
            { "X2_BorderRight", new StringInsertModelExt("X2_BorderRight", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right}, null)},
            { "X2_BorderLeft", new StringInsertModelExt("X2_BorderLeft", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Left}, null)},
            { "X4_BorderLeft", new StringInsertModelExt("X4_BorderLeft", ":X4", new BorderSubString{DelimiterSign = DelimiterSign.Left}, null)},

            //DEL
            { "X2_BorderRight<01>", new StringInsertModelExt("X2_BorderRight<01>", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right, StartCh = "01", StartInclude = true}, null)},
            //DEL
            { "X2_BorderRight<02>", new StringInsertModelExt("X2_BorderRight<02>", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right, StartCh = "02", StartInclude = true}, null)},

            { "X2_BorderLeft<0x02-0x03>", new StringInsertModelExt("X2_BorderLeft<0x02-0x03>", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Left, StartCh = "0x02", EndCh = "0x03", StartInclude = false, EndInclude = false}, null)},
            { "X4_BorderLeft<0x02-0x03>", new StringInsertModelExt("X4_BorderLeft<0x02-0x03>", ":X4", new BorderSubString{DelimiterSign = DelimiterSign.Left, StartCh = "0x02", EndCh = "0x03", StartInclude = false, EndInclude = false}, null)},


            { "X2_Border", new StringInsertModelExt("X2_Border", ":X2", new BorderSubString{StartCh = "0x02", EndCh = "0x03", StartInclude = true}, null)},
            { "X2_BorderExclude", new StringInsertModelExt("X2_BorderExclude", ":X2", new BorderSubString{StartCh = "0x02", EndCh = "0x03", StartInclude = false}, null)},
            { "X4_Border", new StringInsertModelExt("X4_Border", ":X4", new BorderSubString{StartCh = "0x02", EndCh = "0x03", StartInclude = false}, null)},
            { "X2_BorderIncl_Sinergo", new StringInsertModelExt("X2_BorderIncl_Sinergo", ":X2", new BorderSubString{StartCh = ":", EndCh = "*", StartInclude = true, EndInclude = true}, null)},
            { "X2_hex_Border_StartOnly", new StringInsertModelExt("X2_hex_Border_StartOnly", ":X2", new BorderSubString{StartCh = "01", StartInclude = true}, null)},
            { "X2_hex_Border_StartOnly2", new StringInsertModelExt("X2_hex_Border_StartOnly2", ":X2", new BorderSubString{StartCh = "02", StartInclude = true}, null)},
            { "X2_Border_StartOnly", new StringInsertModelExt("X2_Border_StartOnly", ":X2", new BorderSubString{StartCh = "0x01", StartInclude = true}, null)},

            //With string MW
            { "MW_Limit(2)", new StringInsertModelExt("MW_Limit(2)", null, null, new StringHandlerMiddleWareOption
            {
                Converters = new List<UnitStringConverterOption>
                {
                    new UnitStringConverterOption {LimitStringConverterOption = new LimitStringConverterOption{Limit = 2}}
                }
            })},
            { "MW_PadR(6)", new StringInsertModelExt("MW_PadR(6)", null, null, new StringHandlerMiddleWareOption
            {
                Converters = new List<UnitStringConverterOption>
                {
                    new UnitStringConverterOption {PadRightStringConverterOption = new PadRightStringConverterOption{Lenght = 6}}
                }
            })},
            { "t_MW_PadR(10)", new StringInsertModelExt("t_MW_PadR(10)", ":t", null, new StringHandlerMiddleWareOption
            {
                Converters = new List<UnitStringConverterOption>
                {
                    new UnitStringConverterOption {PadRightStringConverterOption = new PadRightStringConverterOption{Lenght = 10}}
                }
            })},
            { "t_MW_PadR(11)", new StringInsertModelExt("t_MW_PadR(11)", ":t", null, new StringHandlerMiddleWareOption
            {
                Converters = new List<UnitStringConverterOption>
                {
                    new UnitStringConverterOption {PadRightStringConverterOption = new PadRightStringConverterOption{Lenght = 11}}
                }
            })},
            { "MW_Limit(2)->PadR(6)", new StringInsertModelExt("MW_Limit(2)->PadR(6)", null, null, new StringHandlerMiddleWareOption
            {
                Converters = new List<UnitStringConverterOption>
                {
                    new UnitStringConverterOption {LimitStringConverterOption = new LimitStringConverterOption{Limit = 2}},
                    new UnitStringConverterOption {PadRightStringConverterOption = new PadRightStringConverterOption{Lenght = 6}}
                }
            })},
        });
    }
}