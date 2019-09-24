using System;

namespace Infrastructure.Produser.AbstractProduser.Options
{
    public class BaseProduserOption
    {
        public string Key { get; set; }
        public TimeSpan TimeRequest { get; set; }
        public int TrottlingQuantity { get; set; }
    }
}