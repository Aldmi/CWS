using Shared.MiddleWares.HandlersOption;

namespace Domain.Device.MiddleWares4InData.Handlers4InData
{
    public class StringHandlerMiddleWare4InData : BaseHandlerMiddleWare4InData<string>
    {
        #region ctor
        public StringHandlerMiddleWare4InData(StringHandlerMiddleWare4InDataOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }


    public class StringHandlerMiddleWare4InDataOption : StringHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
    }
}