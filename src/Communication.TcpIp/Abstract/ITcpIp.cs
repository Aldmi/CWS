using DAL.Abstract.Entities.Options.Transport;
using Transport.Base.Abstract;

namespace Transport.TcpIp.Abstract
{
    public interface ITcpIp : ITransport
    {
        TcpIpOption Option { get; }                                                           //НАСТРОЙКИ TcpIp
    }
}