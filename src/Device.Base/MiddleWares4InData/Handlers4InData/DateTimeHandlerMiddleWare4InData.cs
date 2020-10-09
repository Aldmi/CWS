using System;
using Shared.MiddleWares.HandlersOption;

namespace Domain.Device.MiddleWares4InData.Handlers4InData
{
    public class DateTimeHandlerMiddleWare4InData : BaseHandlerMiddleWare4InData<DateTime>
    {
        #region ctor
        public DateTimeHandlerMiddleWare4InData(DateTimeHandlerMiddleWare4InDataOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }


    public class DateTimeHandlerMiddleWare4InDataOption : DateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
    }
}