﻿using System;
using System.Collections.Generic;
using System.Linq;
using Shared.MiddleWares.Converters;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.HandlersOption
{
    /// <summary>
    /// Цепочка конверторов для обработки одного св-ва.
    /// </summary>
    public class StringMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
        public List<UnitStringConverterOption> Converters { get; set; }

        public IList<IConverterMiddleWare<string>> CreateConverters()
        {
            return Converters.Select(c => c.CreateConverter()).ToList();
        }
    }

    /// <summary>
    /// Единица обработки строки конвертором
    /// </summary>
    public class UnitStringConverterOption
    {
        public InseartStringConverterOption InseartStringConverterOption { get; set; }
        public LimitStringConverterOption LimitStringConverterOption { get; set; }
        public ReplaceEmptyStringConverterOption ReplaceEmptyStringConverterOption { get; set; }
        public ReplaceSpecStringConverterOption ReplaceSpecStringConverterOption { get; set; }
        public SubStringMemConverterOption SubStringMemConverterOption { get; set; }
        public InseartEndLineMarkerConverterOption InseartEndLineMarkerConverterOption { get; set; }
        public InsertAtEndOfLineConverterOption InsertAtEndOfLineConverterOption { get; set; }
        public PadRightStringConverterOption PadRightStringConverterOption { get; set; }
        public PadRighCharWeightStringConverterOption PadRighCharWeightStringConverterOption { get; set; }

        public IConverterMiddleWare<string> CreateConverter()
        {
            if (InseartStringConverterOption != null) return new InseartStringConverter(InseartStringConverterOption);
            if (LimitStringConverterOption != null) return new LimitStringConverter(LimitStringConverterOption);
            if (ReplaceEmptyStringConverterOption != null) return new ReplaceEmptyStringConverter(ReplaceEmptyStringConverterOption);
            if (ReplaceSpecStringConverterOption != null) return new ReplaceSpecStringConverter(ReplaceSpecStringConverterOption);
            if (SubStringMemConverterOption != null) return new SubStringMemConverter(SubStringMemConverterOption);
            if (InseartEndLineMarkerConverterOption != null) return new InseartEndLineMarkerConverter(InseartEndLineMarkerConverterOption);
            if (InsertAtEndOfLineConverterOption != null) return new InsertAtEndOfLineConverter(InsertAtEndOfLineConverterOption);
            if (PadRightStringConverterOption != null) return new PadRightStringConverter(PadRightStringConverterOption);
            if (PadRighCharWeightStringConverterOption != null) return new PadRightCharWeightStringConverter(PadRighCharWeightStringConverterOption);

            throw new NotSupportedException("В UnitStringConverterOption необходимо указать хотя бы одну опцию");
        }
    }
}