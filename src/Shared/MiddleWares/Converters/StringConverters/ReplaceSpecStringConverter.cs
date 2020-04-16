using System;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Заменяет SpecString на строку ReplacementString.
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