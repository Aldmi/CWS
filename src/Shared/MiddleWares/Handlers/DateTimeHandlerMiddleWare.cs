using System;
using Shared.MiddleWares.HandlersOption;

namespace Shared.MiddleWares.Handlers
{
    public class DateTimeHandlerMiddleWare : BaseHandlerMiddleWare<DateTime>
    {
        #region ctor
        public DateTimeHandlerMiddleWare(DateTimeHandlerMiddleWareOption option)
        {
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }
}