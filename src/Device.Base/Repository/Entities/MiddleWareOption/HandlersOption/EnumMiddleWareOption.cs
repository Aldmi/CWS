﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Device.MiddleWares.Converters;
using Domain.Device.MiddleWares.Converters.EnumsConverters;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption
{
    public class EnumMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

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