using System.ComponentModel.DataAnnotations;

namespace WebServer.DTO.JSON.OptionsDto.TransportOption
{
    public class TcpIpOptionDto
    {
        public int Id { get; set; }

        public bool AutoStartBg { get; set; }

        [Required(ErrorMessage = "Имя TCP/IP  не может быть NULL")]
        public string Name { get; set; }

        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]  //192.168.1.1:5000
        public string IpAddress { get; set; }             //Ip

        public int IpPort { get; set; }                  //порт      
    }
}