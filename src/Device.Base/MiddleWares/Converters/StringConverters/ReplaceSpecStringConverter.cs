using System;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Заменяет SpecString на строку на ReplacementString.
    /// </summary>
    public class ReplaceSpecStringConverter : BaseStringConverter
    {
        private readonly ReplaceSpecStringConverterOption _option;

        public ReplaceSpecStringConverter(ReplaceSpecStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            return string.Equals(inProp, _option.SpecString, StringComparison.CurrentCultureIgnoreCase) 
                ? _option.ReplacementString 
                : inProp;
        }
    }
}