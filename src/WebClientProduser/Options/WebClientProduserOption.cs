using AbstractProduser.Options;
using Shared.Enums;

namespace WebClientProduser.Options
{
    public class WebClientProduserOption : BaseProduserOption
    {
        public string Url { get; set; }
        public HttpMethode HttpMethode { get; set; }
    }

}