﻿using DAL.Abstract.Entities.Options.MiddleWare.Hadlers.StringConvertersOption;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    public class InseartStringConverter : IConverterMiddleWare<string>
    {
        private readonly InseartStringConverterOption _option;

        public InseartStringConverter(InseartStringConverterOption option)
        {
            _option = option;
        }


        public string Convert(string inProp)
        {
            //DEBUG
            return inProp + "After InseartStringConverter";
        }
    }
}