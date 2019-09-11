using DAL.Abstract.Entities.Options.Transport;
using Infrastructure.Transport.Base.Abstract;

namespace Infrastructure.Transport.Http.Abstract
{
    public interface ITcpIp : ITransport
    {
        TcpIpOption Option { get; }                                                           //НАСТРОЙКИ TcpIp
    }
}