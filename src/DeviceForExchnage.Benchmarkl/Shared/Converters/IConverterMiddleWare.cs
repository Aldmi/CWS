namespace DeviceForExchnage.Benchmark.Shared.Converters
{
    public interface IConverterMiddleWare<T>
    {
        T Convert(T inProp);
    }
}