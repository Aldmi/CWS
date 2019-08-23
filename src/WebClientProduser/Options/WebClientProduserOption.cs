using AbstractProduser.Options;

namespace WebClientProduser.Options
{
    public class WebClientProduserOption : BaseProduserOption
    {
        public string Url { get; set; }
        public HttpMethode HttpMethode { get; set; }
    }

    public enum HttpMethode { Get, Post, Put }
}