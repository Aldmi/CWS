using Domain.Device.MiddleWares.Handlers;

namespace Domain.Device.MiddleWares.Converters
{
    public interface IConverterMiddleWare<T>
    {
        int Priority { get; }
        T Convert(T inProp, int dataId); 

    }
}