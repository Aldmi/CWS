using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entities.Options.Device;

namespace DAL.Abstract.Extensions
{
    public static class InitializeDeviceOptionRepository
    {
        public static async Task InitializeAsync(this IDeviceOptionRepository rep)
        {
            //Если есть хотя бы 1 элемент то НЕ иннициализировать
            if (await rep.CountAsync(option=> true) > 0) 
            {
                return;
            }

            var devices = new List<DeviceOption>
            {
                #region Peron

                new DeviceOption
                {
                    Id = 1,
                    Description = "Платформа= 2. ПУТЬ= 10. Столбы= 10,9",
                    Name = "Plat2.P10",
                    TopicName4MessageBroker = "Plat2.P10",
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=200 Plat=2 P=10 Stolb=10 Addr=2",
                        "TcpIp=200 Plat=2 P=10 Stolb=10 Addr=1",

                        "TcpIp=201 Plat=2 P=10 Stolb=9 Addr=5",
                        "TcpIp=201 Plat=2 P=10 Stolb=9 Addr=14",
                    }
                },
                new DeviceOption
                {
                    Id = 2,
                    Description = "Платформа= 2,1. ПУТЬ= 12. Столбы= 10,9,X",
                    Name = "(Plat2&Plat1).P12",
                    TopicName4MessageBroker = "(Plat2&Plat1).P12",
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=200 Plat=2 P=12 Stolb=10 Addr=25",
                        "TcpIp=200 Plat=2 P=12 Stolb=10 Addr=21",

                        "TcpIp=201 Plat=2 P=12 Stolb=9 Addr=6",
                        "TcpIp=201 Plat=2 P=12 Stolb=9 Addr=7",

                        "TcpIp=196 Plat=1 P=12 Stolb=_ Addr=71",
                        "TcpIp=196 Plat=1 P=12 Stolb=_ Addr=72",

                        "TcpIp=197 Plat=1 P=12 Stolb=_ Addr=47",
                        "TcpIp=197 Plat=1 P=12 Stolb=_ Addr=60",

                        "TcpIp=198 Plat=1 P=12 Stolb=_ Addr=57",
                        "TcpIp=198 Plat=1 P=12 Stolb=_ Addr=58",

                        "TcpIp=199 Plat=1 P=12 Stolb=_ Addr=46",
                        "TcpIp=199 Plat=1 P=12 Stolb=_ Addr=59",
                    }
                },
                new DeviceOption
                {
                    Id = 3,
                    Description = "Переход. Платформа= 2. ПУТЬ= 10,12. Столбы= _",
                    Name = "Perehod.P10.P12",
                    TopicName4MessageBroker = "Perehod.P10.P12",
                    AutoBuild = true,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=209 Plat=2 P=10/12 Stolb=_ Addr=9",
                        "TcpIp=210 Plat=2 P=10/12 Stolb=_ Addr=70"
                    }
                },
                new DeviceOption
                {
                    Id = 10,
                    Description = "Платформа= 3. ПУТЬ= 6. Столбы= 7,6,5",
                    Name = "Plat3.P6",
                    TopicName4MessageBroker = "Plat3.P6",
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=203 Plat=3 P=6 Stolb=7 Addr=49",
                        "TcpIp=203 Plat=3 P=6 Stolb=7 Addr=51",

                        "TcpIp=204 Plat=3 P=6 Stolb=6 Addr=17",
                        "TcpIp=204 Plat=3 P=6 Stolb=6 Addr=19",

                        "TcpIp=205 Plat=3 P=6 Stolb=5 Addr=61",
                        "TcpIp=205 Plat=3 P=6 Stolb=5 Addr=62",
                    }
                },
                new DeviceOption
                {
                    Id = 11,
                    Description = "Платформа= 3. ПУТЬ= 8. Столбы= 7,6,5",
                    Name = "Plat3.P8",
                    TopicName4MessageBroker = "Plat3.P8",
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=203 Plat=3 P=8 Stolb=7 Addr=50",
                        "TcpIp=203 Plat=3 P=8 Stolb=7 Addr=53",

                        "TcpIp=204 Plat=3 P=8 Stolb=6 Addr=54",
                        "TcpIp=204 Plat=3 P=8 Stolb=6 Addr=56",

                        "TcpIp=205 Plat=3 P=8 Stolb=5 Addr=63",
                        "TcpIp=205 Plat=3 P=8 Stolb=5 Addr=73",
                    }
                },
                new DeviceOption
                {
                    Id = 12,
                    Description = "Переход. Платформа= 3. ПУТЬ= 6,8. Столбы= _",
                    Name = "Perehod.P6.P8",
                    TopicName4MessageBroker = "Perehod.P6.P8",
                    AutoBuild = true,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=209 Plat=3 P=6/8 Stolb=_ Addr=55",
                        "TcpIp=210 Plat=3 P=6/8 Stolb=_ Addr=52"
                    }
                },
                new DeviceOption
                {
                    Id = 20,
                    Description = "Платформа= 4. ПУТЬ= 5. Столбы= 4,3,2",
                    Name = "Plat4.P5",
                    TopicName4MessageBroker = "Plat4.P5",
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=206 Plat=4 P=5 Stolb=4 Addr=8",
                        "TcpIp=206 Plat=4 P=5 Stolb=4 Addr=11",

                        "TcpIp=207 Plat=4 P=5 Stolb=3 Addr=4",
                        "TcpIp=207 Plat=4 P=5 Stolb=3 Addr=13",

                        "TcpIp=208 Plat=4 P=5 Stolb=2 Addr=10",
                        "TcpIp=208 Plat=4 P=5 Stolb=2 Addr=18",
                    }
                },
                new DeviceOption
                {
                    Id = 21,
                    Description = "Платформа= 4. ПУТЬ= 3. Столбы= 4,3,2",
                    Name = "Plat4.P3",
                    TopicName4MessageBroker = "Plat4.P3",
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=206 Plat=4 P=3 Stolb=4 Addr=26",
                        "TcpIp=206 Plat=4 P=3 Stolb=4 Addr=44",

                        "TcpIp=207 Plat=4 P=3 Stolb=3 Addr=23",
                        "TcpIp=207 Plat=4 P=3 Stolb=3 Addr=29",

                        "TcpIp=208 Plat=4 P=3 Stolb=2 Addr=3",
                        "TcpIp=208 Plat=4 P=3 Stolb=2 Addr=12",
                    }
                },
                new DeviceOption
                {
                    Id = 22,
                    Description = "Платформа= 4. ПУТЬ= 3,5. Столбы= _",
                    Name = "Perehod.P3.P5",
                    TopicName4MessageBroker = "Perehod.P3.P5",
                    AutoBuild = true,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=209 Plat=4 P=3/5 Stolb=_ Addr=45",
                        "TcpIp=210 Plat=4 P=3/5 Stolb=_ Addr=48"
                    }
                },

                #endregion


                #region MultiStr

                new DeviceOption
                {
                    Id = 50,
                    Description = "9 строк табло. Прибытие/Отправление",
                    Name = "PribOtpr_9Str_4Table",
                    TopicName4MessageBroker = "PribOtpr_9Str_4Table",
                    AutoBuild = true,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp=194 Event=PribOtpr NItem=9 Addr=64",

                        //"TcpIp=195 Event=PribOtpr NItem=9 Addr=65",
                        //"TcpIp=195 Event=PribOtpr NItem=9 Addr=66",
                        //"TcpIp=195 Event=PribOtpr NItem=9 Addr=67",
                    }
                },

                #endregion
            };

            await rep.AddRangeAsync(devices);
        }
    }
}