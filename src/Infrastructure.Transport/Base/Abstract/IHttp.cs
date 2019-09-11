using DAL.Abstract.Entities.Options.Transport;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface IHttp :  ITransport
    {
        HttpOption Option { get; }                                                           //НАСТРОЙКИ Http
    }
}