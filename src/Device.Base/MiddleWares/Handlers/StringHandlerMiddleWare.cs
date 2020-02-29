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

        public StringHandlerMiddleWare(StringHandlerMiddleWareOption option)
        {
            PropName = option.PropName;

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