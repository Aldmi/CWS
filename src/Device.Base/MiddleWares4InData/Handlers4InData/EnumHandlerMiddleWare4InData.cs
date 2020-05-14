using System;
using Domain.Device.Repository.Entities.MiddleWareOption;

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
}