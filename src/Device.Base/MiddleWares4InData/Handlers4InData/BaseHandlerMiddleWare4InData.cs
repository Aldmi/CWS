using Shared.MiddleWares.Handlers;

namespace Domain.Device.MiddleWares4InData.Handlers4InData
{
    public class BaseHandlerMiddleWare4InData<T> : BaseHandlerMiddleWare<T>
    {
        public string PropName { get; protected set; }
    }
}