using DAL.Abstract.Entities.Options.Transport;

namespace Infrastructure.Transport.TcpIp
{
    public class TcpIpOption : BackgroundOption
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }             //Ip
        public int IpPort { get; set; }                  //порт          
    }
}
