using Infrastructure.Transport.SibWay;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface ISibWay : ITransport
    {
         SibWayTransportOption Option { get; }   //Настройки SibWay
    }
}