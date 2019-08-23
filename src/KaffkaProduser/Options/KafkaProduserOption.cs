using AbstractProduser.Options;

namespace KafkaProduser.Options
{
    public class KafkaProduserOption : BaseProduserOption
    {
        public string BrokerEndpoints { get; set; }
        public string TopicName { get; set; }
    }
}