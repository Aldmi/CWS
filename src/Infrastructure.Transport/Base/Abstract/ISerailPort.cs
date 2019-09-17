using Infrastructure.Transport.SerialPort;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface ISerailPort : ITransport
    {
        SerialOption Option { get; }                                                           //НАСТРОЙКИ ПОРТА
    }
}