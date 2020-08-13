using Shared.Enums;

namespace Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers
{
    public class EfWebClientProduserOption : EfBaseProduserOption
    {
        public string InitUrl { get; set; }
        public string BoardDataUrl { get; set; }
        public string InfoUrl { get; set; }
        public string WarningUrl { get; set; }

        public HttpMethode HttpMethode { get; set; }
    }
}