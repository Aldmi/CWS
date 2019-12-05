namespace Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers
{
    public class EfKafkaProduserOption : EfBaseProduserOption
    {
        public string BrokerEndpoints { get; set; }
        public string TopicName { get; set; }
    }
}