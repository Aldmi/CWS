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
    public class StringHandlerMiddleWareOption
    {
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
        public LimitStringConverterOption? LimitStringConverterOption { get; set; }
        public SubStringMemConverterOption? SubStringMemConverterOption { get; set; }
        public InseartEndLineMarkerConverterOption? InseartEndLineMarkerConverterOption { get; set; }
        public InsertAtEndOfLineConverterOption? InsertAtEndOfLineConverterOption { get; set; }
        public PadRightStringConverterOption? PadRightStringConverterOption { get; set; }
        public PadRighCharWeightStringConverterOption? PadRighCharWeightStringConverterOption { get; set; }
        public ReplaseStringConverterOption? ReplaseStringConverterOption { get; set; }
        public ReplaseCharStringConverterOption? ReplaseCharStringConverterOption { get; set; }
        public ToLowerConverterOption? ToLowerConverterOption { get; set; }
        public ToUpperConverterOption? ToUpperConverterOption { get; set; }
        public PadRighOptimalFillingConverterOption? PadRighOptimalFillingConverterOption { get; set; }
        public SubStringConverterOption? SubStringConverterOption { get; set; }
        public PadRightStrStringConverterOption? PadRightStrStringConverterOption { get; set; }

        public IConverterMiddleWare<string> CreateConverter()
        {
            if (LimitStringConverterOption != null) return new LimitStringConverter(LimitStringConverterOption);
            if (SubStringMemConverterOption != null) return new SubStringMemConverter(SubStringMemConverterOption);
            if (InseartEndLineMarkerConverterOption != null) return new InseartEndLineMarkerConverter(InseartEndLineMarkerConverterOption);
            if (InsertAtEndOfLineConverterOption != null) return new InsertAtEndOfLineConverter(InsertAtEndOfLineConverterOption);
            if (PadRightStringConverterOption != null) return new PadRightStringConverter(PadRightStringConverterOption);
            if (PadRighCharWeightStringConverterOption != null) return new PadRightCharWeightStringConverter(PadRighCharWeightStringConverterOption);
            if (ReplaseStringConverterOption != null) return new ReplaseStringConverter(ReplaseStringConverterOption);
            if (ReplaseCharStringConverterOption != null) return new ReplaseCharStringConverter(ReplaseCharStringConverterOption);
            if (ToLowerConverterOption != null) return new ToLowerConverter(ToLowerConverterOption);
            if (ToUpperConverterOption != null) return new ToUpperConverter(ToUpperConverterOption);
            if (PadRighOptimalFillingConverterOption != null) return new PadRighOptimalFillingConverter(PadRighOptimalFillingConverterOption);
            if (SubStringConverterOption != null) return new SubStringConverter(SubStringConverterOption);
            if (PadRightStrStringConverterOption != null) return new PadRightStrStringConverter(PadRightStrStringConverterOption);

            throw new NotSupportedException("В UnitStringConverterOption необходимо указать хотя бы одну опцию");
        }
    }
}