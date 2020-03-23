using System.Collections.Generic;
using System.Linq;
using Domain.Device.MiddleWares.Converters.StringConverters;
using StringHandlerMiddleWareOption = Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption.StringHandlerMiddleWareOption;

namespace Domain.Device.MiddleWares.Handlers
{
    /// <summary>
    /// Обработчик строковой переменной.
    /// Содержит имя переенной и цепочку обработчиков.
    /// </summary>
    public class StringHandlerMiddleWare : BaseHandlerMiddleWare<string>
    {
        #region ctor

        public StringHandlerMiddleWare(string propName, params StringHandlerMiddleWareOption[] options)
        {
            PropName = propName;
            foreach (var option in options)
            {
                if (option.InseartStringConverterOption != null)
                {
                    Converters.Add(new InseartStringConverter(option.InseartStringConverterOption));
                }

                if (option.LimitStringConverterOption != null)
                {
                    Converters.Add(new LimitStringComverter(option.LimitStringConverterOption));
                }

                if (option.ReplaceEmptyStringConverterOption != null)
                {
                    Converters.Add(new ReplaceEmptyStringConverter(option.ReplaceEmptyStringConverterOption));
                }

                if (option.ReplaceSpecStringConverterOption != null)
                {
                    Converters.Add(new ReplaceSpecStringConverter(option.ReplaceSpecStringConverterOption));
                }

                if (option.InseartEndLineMarkerConverterOption != null)
                {
                    Converters.Add(new InseartEndLineMarkerConverter(option.InseartEndLineMarkerConverterOption));
                }

                if (option.SubStringMemConverterOption != null)
                {
                    Converters.Add(new SubStringMemConverter(option.SubStringMemConverterOption));
                }

                if (option.InsertAtEndOfLineConverterOption != null)
                {
                    Converters.Add(new InsertAtEndOfLineConverter(option.InsertAtEndOfLineConverterOption));
                }

                if (option.PadRightStringConverterOption != null)
                {
                    Converters.Add(new PadRightStringConverter(option.PadRightStringConverterOption));
                }

                if (option.PadRighCharWeightStringConverterOption != null)
                {
                    Converters.Add(new PadRightCharWeightStringConverter(option.PadRighCharWeightStringConverterOption));
                }
            }
            var orderedConverters = Converters.OrderBy(c => c.Priority).ToList();
            Converters.Clear();
            Converters.AddRange(orderedConverters);
        }



        //private bool AddConverter(BaseConverterOption converterOption)
        //{
        //    if (converterOption == null)
        //        return false;

        //}

        #endregion
    }
}