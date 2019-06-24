﻿using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    public class ReplaceEmptyStringConverter : BaseStringConverter
    {

        private readonly ReplaceEmptyStringConverterOption _option;

        public ReplaceEmptyStringConverter(ReplaceEmptyStringConverterOption option)
            : base(option)
        {
            _option = option;
        }



        protected override string ConvertChild(string inProp, int dataId)
        {
            //DEBUG
            return inProp + "After ReplaceEmptyStringConverter";
        }
    }
}