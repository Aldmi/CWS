using DAL.Abstract.Entities.Options.Transport;
using Transport.Base.Abstract;

namespace Transport.Http.Abstract
{
    public interface IHttp :  ITransport
    {
        HttpOption Option { get; }                                                           //НАСТРОЙКИ Http
    }
}