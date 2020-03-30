using Domain.Device.MiddleWares.Handlers;

namespace Domain.Device.MiddleWares.Converters
{
    public interface IMemConverterMiddleWare
    {
        void SendCommand(MemConverterCommand command);
    }

    public enum MemConverterCommand { None, Reset }
}