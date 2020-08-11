using Infrastructure.Produser.AbstractProduser.Options;

namespace Domain.Device.Repository.Entities.ResponseProduser
{
    public class SignalRProduserOption : BaseProduserOption
    {
        public string InitMethodeName { get; set; }
        public string BoardDataMethodeName { get; set; }
        public string InfoMethodeName { get; set; }
        public string WarningMethodeName { get; set; }
    }
}