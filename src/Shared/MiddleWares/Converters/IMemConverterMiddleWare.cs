namespace Shared.MiddleWares.Converters
{
    public interface IMemConverterMiddleWare
    {
        void SendCommand(MemConverterCommand command);
    }

    public enum MemConverterCommand { None, Reset }
}