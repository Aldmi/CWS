
namespace Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers
{
    public class EfSignalRProduserOption : EfBaseProduserOption
    {
        public string InitMethodeName { get; set; }
        public string BoardDataMethodeName { get; set; }
        public string InfoMethodeName { get; set; }
        public string WarningMethodeName { get; set; }
    }
}