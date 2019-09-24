using Infrastructure.Produser.AbstractProduser.Options;
using Shared.Enums;

namespace Infrastructure.Produser.WebClientProduser.Options
{
    public class WebClientProduserOption : BaseProduserOption
    {
        public string Url { get; set; }
        public HttpMethode HttpMethode { get; set; }
    }

}