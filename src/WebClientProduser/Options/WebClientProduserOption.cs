using Infrastructure.Produser.AbstractProduser.Options;
using Shared.Enums;

namespace Infrastructure.Produser.WebClientProduser.Options
{
    public class WebClientProduserOption : BaseProduserOption
    {
        public string InitUrl { get; set; }
        public string BoardDataUrl { get; set; }
        public string InfoUrl { get; set; }
        public string WarningUrl { get; set; }

        public HttpMethode HttpMethode { get; set; }
    }
}