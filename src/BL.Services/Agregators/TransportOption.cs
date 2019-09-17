using System.Collections.Generic;
using Infrastructure.Transport.Http;
using Infrastructure.Transport.SerialPort;
using Infrastructure.Transport.TcpIp;

namespace App.Services.Agregators
{

    public class TransportOption
    {
        public IList<SerialOption> SerialOptions { get; set; }
        public IList<TcpIpOption> TcpIpOptions { get; set; }
        public IList<HttpOption> HttpOptions { get; set; }
    }
}