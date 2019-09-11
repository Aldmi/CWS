using DAL.Abstract.Entities.Options.Transport;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface ITcpIp : ITransport
    {
        TcpIpOption Option { get; }                                                           //НАСТРОЙКИ TcpIp
    }
}