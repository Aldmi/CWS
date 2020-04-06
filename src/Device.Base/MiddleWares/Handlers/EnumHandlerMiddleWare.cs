﻿using System;
using System.Linq;
using Domain.Device.MiddleWares.Converters.EnumsConverters;
using Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption;

namespace Domain.Device.MiddleWares.Handlers
{
    /// <summary>
    /// Обработчик byte переменной.
    /// Содержит имя переменной и цепочку обработчиков.
    /// </summary>
    public class EnumHandlerMiddleWare : BaseHandlerMiddleWare<Enum>
    {
        #region ctor
        public EnumHandlerMiddleWare(string propName, params EnumHandlerMiddleWareOption[] options)
        {
            PropName = propName;
            foreach (var option in options)
            {
                if (option.EnumMemConverterOption != null)
                {
                    Converters.Add(new EnumerateConverter(option.EnumMemConverterOption));
                }
            }
        }
        #endregion
    }
}