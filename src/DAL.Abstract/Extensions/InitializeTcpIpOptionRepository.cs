using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entities.Options.Transport;

namespace DAL.Abstract.Extensions
{
    public static class InitializeTcpIpOptionRepository
    {
        public static async Task InitializeAsync(this ITcpIpOptionRepository rep)
        {
            //Если есть хотя бы 1 элемент то НЕ иннициализировать
            if (await rep.CountAsync(option=> true) > 0) 
            {
                return;
            }

            var tcpIpList = new List<TcpIpOption>
            {
                new TcpIpOption
                {
                   Id=1,
                   Name = "TcpIp=194",
                   IpAddress = "10.27.15.194",
                   IpPort = 4001,
                   AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=2,
                    Name = "TcpIp=195",
                    IpAddress = "10.27.15.195",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=3,
                    Name = "TcpIp=196",
                    IpAddress = "10.27.15.196",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=4,
                    Name = "TcpIp=197",
                    IpAddress = "10.27.15.197",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=5,
                    Name = "TcpIp=198",
                    IpAddress = "10.27.15.198",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=6,
                    Name = "TcpIp=199",
                    IpAddress = "10.27.15.199",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=7,
                    Name = "TcpIp=200",
                    IpAddress = "10.27.15.200",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=8,
                    Name = "TcpIp=201",
                    IpAddress = "10.27.15.201",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                //new TcpIpOption       //СТОЛБ 9(не работает)
                //{
                //    Id=9,
                //    Name = "TcpIp=202",
                //    IpAddress = "10.27.15.202",
                //    IpPort = 4001,
                //    AutoStartBg = true,
                //},
                new TcpIpOption
                {
                    Id=10,
                    Name = "TcpIp=203",
                    IpAddress = "10.27.15.203",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=11,
                    Name = "TcpIp=204",
                    IpAddress = "10.27.15.204",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=12,
                    Name = "TcpIp=205",
                    IpAddress = "10.27.15.205",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=13,
                    Name = "TcpIp=206",
                    IpAddress = "10.27.15.206",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=14,
                    Name = "TcpIp=207",
                    IpAddress = "10.27.15.207",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=15,
                    Name = "TcpIp=208",
                    IpAddress = "10.27.15.208",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=16,
                    Name = "TcpIp=209",
                    IpAddress = "10.27.15.209",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
                new TcpIpOption
                {
                    Id=17,
                    Name = "TcpIp=210",
                    IpAddress = "10.27.15.210",
                    IpPort = 4001,
                    AutoStartBg = true,
                },
            };

            await rep.AddRangeAsync(tcpIpList);
        }
    }
}