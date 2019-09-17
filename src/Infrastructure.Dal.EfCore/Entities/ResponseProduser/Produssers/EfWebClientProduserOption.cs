namespace Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers
{
    public class EfWebClientProduserOption : EfBaseProduserOption
    {
        public string Url { get; set; }
        public HttpMethode HttpMethode { get; set; }
    }

    public enum HttpMethode { Get, Post, Put }  //TODO: вынести в Shared
}