namespace DeviceForExchange.MiddleWares.Converters
{
    public interface IConverterMiddleWare<T>
    {
        int Priority { get; }
        T Convert(T inProp, int dataId);
    }
}