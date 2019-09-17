using System;

namespace Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers
{
    public class EfBaseProduserOption
    {
        public string Key { get; set; }
        public TimeSpan TimeRequest { get; set; }
        public int TrottlingQuantity { get; set; }
    }
}