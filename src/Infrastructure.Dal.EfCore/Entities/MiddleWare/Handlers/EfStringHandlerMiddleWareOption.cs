﻿using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfStringHandlerMiddleWareOption
    {
        public List<EfUnitStringConverterOption> Converters { get; set; }
    }

    public class EfUnitStringConverterOption
    {
        public EfLimitStringConverterOption LimitStringConverterOption { get; set; }
        public EfSubStringMemConverterOption SubStringMemConverterOption { get; set; }
        public EfInseartEndLineMarkerConverterOption InseartEndLineMarkerConverterOption { get; set; }
        public EfInsertAtEndOfLineConverterOption InsertAtEndOfLineConverterOption { get; set; }
        public EfPadRightStringConverterOption PadRightStringConverterOption { get; set; }
        public EfPadRighCharWeightStringConverterOption PadRighCharWeightStringConverterOption { get; set; }
        public EfReplaseStringConverterOption ReplaseStringConverterOption { get; set; }
        public EfReplaseCharStringConverterOption ReplaseCharStringConverterOption { get; set; }
        public EfToLowerConverterOption ToLowerConverterOption { get; set; }
        public EfToUpperConverterOption ToUpperConverterOption { get; set; }
        public EfPadRighOptimalFillingConverterOption PadRighOptimalFillingConverterOption { get; set; }
        public EfSubStringConverterOption SubStringConverterOption { get; set; }
        public EfPadRightStrStringConverterOption PadRightStrStringConverterOption { get; set; }
    }
}