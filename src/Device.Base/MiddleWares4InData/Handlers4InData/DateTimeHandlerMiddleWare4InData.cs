﻿using System;
using Domain.Device.Repository.Entities.MiddleWareOption;

namespace Domain.Device.MiddleWares4InData.Handlers4InData
{
    public class DateTimeHandlerMiddleWare4InData : BaseHandlerMiddleWare4InData<DateTime>
    {
        #region ctor
        public DateTimeHandlerMiddleWare4InData(DateTimeHandlerHandlerMiddleWare4InDataOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }
}