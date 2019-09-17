using System.IO.Ports;
using DAL.Abstract.Entities.Options.Transport;

namespace Infrastructure.Transport.SerialPort
{
    public class SerialOption : BackgroundOption
    {
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }
    }
}