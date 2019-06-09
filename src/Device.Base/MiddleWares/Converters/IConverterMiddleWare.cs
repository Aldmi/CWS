namespace DeviceForExchange.MiddleWares.Converters
{
    public interface IConverterMiddleWare<T>
    {
        T Convert(T inProp);
    }
}