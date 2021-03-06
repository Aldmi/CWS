﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Mathematic;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.HandlersOption;
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
            { "X4", new StringInsertModelExt("X4", ":X4", null, null)},
            { "D3", new StringInsertModelExt("D3", ":D3", null, null)},
            { "D2", new StringInsertModelExt("D2", ":D2", null, null)},
            { "t", new StringInsertModelExt("t", ":t", null, null)},

            { "D2_BorderRightBeforeCrc", new StringInsertModelExt("D2_BorderRightBeforeCrc", ":D2", new BorderSubString{DelimiterSign = DelimiterSign.Right, EndCh = "{CRC", EndInclude = false}, null)},
            { "X2_BorderRightBeforeCrc", new StringInsertModelExt("X2_BorderRightBeforeCrc", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right, EndCh = "{CRC", EndInclude = false}, null)},

            { "X2_BorderRightBeforeCrc_Math", new StringInsertModelExt("X2_BorderRightBeforeCrc_Math", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right, EndCh = "{CRC", EndInclude = false}, null, new MathematicFormula("x+3"))},
            { "X2_BorderRight_Math", new StringInsertModelExt("X2_BorderRight_Math", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right}, null, new MathematicFormula("x+1"))},

            { "X2_BorderRight", new StringInsertModelExt("X2_BorderRight", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right}, null)},
            { "X2_BorderLeft", new StringInsertModelExt("X2_BorderLeft", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Left}, null)},
            { "X4_BorderLeft", new StringInsertModelExt("X4_BorderLeft", ":X4", new BorderSubString{DelimiterSign = DelimiterSign.Left}, null)},

            { "D2_NcharBorder<%01-%1>", new StringInsertModelExt("D2_NcharBorder<%01-%1>", ":D2", new BorderSubString{DelimiterSign = DelimiterSign.Right, StartCh = "%01", EndCh = "%1", StartInclude = false, EndInclude = false}, null)},
            { "X2_NbyteFull<%01-%1>_Math", new StringInsertModelExt("X2_NbyteFull<%01-%1>_Math", ":X2", new BorderSubString{DelimiterSign = DelimiterSign.Right, StartCh = "%01", EndCh = "%1", StartInclude = false, EndInclude = false}, null, new MathematicFormula("x+3"))},

            { "X2_Border", new StringInsertModelExt("X2_Border", ":X2", new BorderSubString{StartCh = "0x02", EndCh = "0x03", StartInclude = true, EndInclude = true}, null)},
            { "X4_Border", new StringInsertModelExt("X4_Border", ":X4", new BorderSubString{StartCh = "0x02", EndCh = "0x03", StartInclude = false}, null)},

            { "X2_Border_StartOnly", new StringInsertModelExt("X2_Border_StartOnly", ":X2", new BorderSubString{StartCh = "0x01", StartInclude = true}, null)},

            { "X2_Border[0x03-10]", new StringInsertModelExt("X2_Border[0x03-10]", ":X2", new BorderSubString{StartCh = "0x03", EndCh ="10", StartInclude = true, EndInclude = true}, null)},

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