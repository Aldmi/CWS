using System;
using System.Collections.Generic;
using System.Linq;
using Shared.MiddleWares.Converters;
using Shared.MiddleWares.Converters.EnumsConverters;
using Shared.MiddleWares.ConvertersOption.EnumsConvertersOption;

namespace Shared.MiddleWares.HandlersOption
{
    public class EnumHandlerMiddleWareOption
    {
        /// <summary>
        /// Полный путь до типа в сборки.
        /// Вида: "Domain.InputDataModel.Autodictor.Entities.Lang, Domain.InputDataModel.Autodictor"
        /// </summary>
        public string Path2Type { get; set; }

        public List<UnitEnumConverterOption> Converters { get; set; }


        public IList<IConverterMiddleWare<Enum>> CreateConverters()
        {
           return Converters.Select(c => c.CreateConverter(Path2Type)).ToList();
        }
    }


    /// <summary>
    /// Единица обработки Enum конвертором
    /// </summary>
    public class UnitEnumConverterOption
    {
        public EnumMemConverterOption EnumMemConverterOption { get; set; }


        public IConverterMiddleWare<Enum> CreateConverter(string path2Type)
        {
            if (EnumMemConverterOption != null) return new EnumerateConverter(EnumMemConverterOption, path2Type);

            throw new NotSupportedException("В UnitEnumConverterOption необходимо указать хотя бы одну опцию");
        }
    }
}