using Infrastructure.Transport.Http;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface IHttp :  ITransport
    {
        HttpOption Option { get; }                                                           //НАСТРОЙКИ Http
    }
}