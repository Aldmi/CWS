using TcpIpOption = Infrastructure.Transport.TcpIp.TcpIpOption;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface ITcpIp : ITransport
    {
        TcpIpOption Option { get; }                                                           //НАСТРОЙКИ TcpIp
    }
}