using System;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;
using Domain.InputDataModel.Autodictor.Entities;

namespace Domain.Device.MiddleWares.Converters.EnumsConverters
{
    public class EnumerateConverter : BaseEnumConverter
    {
        private readonly EnumerateConverterOption _option;
        public EnumerateConverter(EnumerateConverterOption option)
            : base(option)
        {
            _option = option;
        }


        public override Enum Convert(Enum inProp, int dataId)
        {
            var objName = "Lang";
            //Создание объекта по имени

           var hh= inProp is Lang;
           var lang = (Lang)inProp;
           lang = Lang.Eng;
           return lang;
        }
    }
}