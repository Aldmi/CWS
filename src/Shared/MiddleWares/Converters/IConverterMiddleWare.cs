namespace Shared.MiddleWares.Converters
{
    public interface IConverterMiddleWare<T>
    {
        T Convert(T inProp, int dataId);
    }
}