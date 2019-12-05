using Infrastructure.Produser.AbstractProduser.Options;

namespace Infrastructure.Produser.KafkaProduser.Options
{
    public class KafkaProduserOption : BaseProduserOption
    {
        public string BrokerEndpoints { get; set; }
        public string TopicName { get; set; }
    }
}