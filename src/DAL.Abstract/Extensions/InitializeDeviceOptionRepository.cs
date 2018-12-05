﻿using System;
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

            //var devices = new List<DeviceOption>
            //{
            //    new DeviceOption
            //    {
            //        Id = 1,
            //        Description = "Табло1",
            //        Name = "Vidor1",
            //        AutoBuild = false,
            //        ExchangeKeys = new List<string>
            //        {
            //            "SP_COM1_Vidor1",
            //            "SP_COM2_Vidor2"
            //        }
            //    },
            //    new DeviceOption
            //    {
            //        Id = 2,
            //        Description = "Табло2",
            //        Name = "HttpTable_google",
            //        AutoBuild = false,
            //        ExchangeKeys = new List<string>
            //        {
            //            "HTTP_google.com_Table1"
            //        }
            //    },
            //    new DeviceOption
            //    {
            //        Id = 3,
            //        Description = "Табло3",
            //        Name = "TcpIp Table1",
            //        AutoBuild = false,
            //        ExchangeKeys = new List<string>
            //        {
            //            "TcpIp_Table1"
            //        }
            //    }
            //};

            var devices = new List<DeviceOption>
            {
                new DeviceOption
                {
                    Id = 1,
                    Description = "Табло1",
                    Name = "Vidor1",
                    TopicName4MessageBroker = "Vidor1", //String.Empty
                    AutoBuild = false,  //true
                    ExchangeKeys = new List<string>
                    {
                        //"SP_COM1_Vidor1",
                        "TcpIp_table_46",
                        "TcpIp_table_59",
                        "TcpIp_table_NONE"
                    }
                },
                new DeviceOption
                {
                    Id = 2,
                    Description = "Табло2",
                    Name = "Vidor9Str",
                    TopicName4MessageBroker = "Vidor9Str", //String.Empty
                    AutoBuild = true,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp_table_PribOtpr9Str"
                    }
                },
                new DeviceOption
                {
                    Id = 3,
                    Description = "Табло3",
                    Name = "TestPeronn_70_52_48",
                    TopicName4MessageBroker = "TestPeronn", //String.Empty
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp_table_TestPeronn_70",
                        "TcpIp_table_TestPeronn_52",
                        "TcpIp_table_TestPeronn_48"
                    }
                },
                new DeviceOption
                {
                    Id = 4,
                    Description = "Табло3",
                    Name = "TestPeronn_45_55_9",
                    TopicName4MessageBroker = "TestPeronn", //String.Empty
                    AutoBuild = false,
                    ExchangeKeys = new List<string>
                    {
                        "TcpIp_table_TestPeronn_45",
                        "TcpIp_table_TestPeronn_55",
                        //"TcpIp_table_TestPeronn_9",
                        "TcpIp_table_TestPeronn_9_Slim",               
                    }
                },
                //new DeviceOption
                //{
                //    Id = 2,
                //    Description = "Табло1",
                //    Name = "Vidor2",
                //    TopicName4MessageBroker = "Vidor1",
                //    AutoBuild = false,
                //    ExchangeKeys = new List<string>
                //    {
                //        "SP_COM2_Vidor2222",
                //    }
                //}
            };

            await rep.AddRangeAsync(devices);
        }
    }
}