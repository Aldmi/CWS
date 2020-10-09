using System;
using Shared.MiddleWares.HandlersOption;

namespace Domain.Device.MiddleWares4InData.Handlers4InData
{
    public class EnumHandlerMiddleWare4InData : BaseHandlerMiddleWare4InData<Enum>
    {
        #region ctor
        public EnumHandlerMiddleWare4InData(EnumHandlerMiddleWare4InDataOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }

    public class EnumHandlerMiddleWare4InDataOption : EnumHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
    }
}