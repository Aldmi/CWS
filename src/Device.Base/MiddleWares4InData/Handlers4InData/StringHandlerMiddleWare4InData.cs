using Domain.Device.Repository.Entities.MiddleWareOption;


namespace Domain.Device.MiddleWares4InDatas.Handlers
{
    public class StringHandlerMiddleWare4InData : BaseHandlerMiddleWare4InData<string>
    {
        #region ctor
        public StringHandlerMiddleWare4InData(StringHandlerHandlerMiddleWare4InDataOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }
}