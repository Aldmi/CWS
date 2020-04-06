using Domain.Device.MiddleWares.Handlers;

namespace Domain.Device.MiddleWares.Converters
{
    public interface IConverterMiddleWare<T>
    {
        T Convert(T inProp, int dataId);
    }
}