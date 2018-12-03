using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Shared.Enums;
using Shared.Types;

namespace DAL.Abstract.Extensions
{
    public static class InitializeExchangeOptionRepository
    {
        public static async Task InitializeAsync(this IExchangeOptionRepository rep)
        {
            //Если есть хотя бы 1 элемент то НЕ иннициализировать
            if (await rep.CountAsync(option => true) > 0)
            {
                return;
            }

            var exchanges = new List<ExchangeOption>
            {
                //new ExchangeOption
                //{
                //    Id = 1,
                //    Key = "SP_COM1_Vidor1",
                //    KeyTransport = new KeyTransport("COM1", TransportType.SerialPort),
                //    AutoStartCycleFunc = false,
                //    CountBadTrying = 3,
                //    Provider = new ProviderOption
                //    {
                //        Name = "ByRules",
                //        ByRulesProviderOption = new ByRulesProviderOption
                //        {
                //            Rules = new List<RuleOption>
                //            {
                //                new RuleOption
                //                {
                //                    Name = "Rule_1",
                //                    AddressDevice = "10",
                //                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                //                    WhereFilter = "true",
                //                    OrderBy = "Id",
                //                    //OrderBy = "ArrivalTime",
                //                    TakeItems = 1, //2
                                    
                //                    ViewRules = new List<ViewRuleOption>
                //                    {
                //                        new ViewRuleOption
                //                        {
                //                            Id = 1,
                //                            StartPosition = 0,
                //                            Count = 1,
                //                            BatchSize = 1000,
                //                            RequestOption = new RequestOption{Header = "{adress}", Body = "01{adress}0502{Station}", Footer = "{CrcXor16}", MaxBodyLenght = 140, Format = "Windows-1251"},
                //                            ResponseOption = new ResponseOption{Body = "01050A", Lenght = 3, TimeRespone = 3000, Format = "X"}
                //                        },
                //                        //new ViewRuleOption
                //                        //{
                //                        //    Id = 2,
                //                        //    StartPosition = 1,
                //                        //    Count = 1,
                //                        //    BatchSize = 1000,
                //                        //    RequestOption = new RequestOption{Header = "{adress}", Body = "01{adress}05026{ArrivalTime}2158{Station}", Footer = "{CrcXor16}", MaxLenght = 1000, Format = "Windows-1251"},
                //                        //    ResponseOption = new ResponseOption{Body = "01050A0606", MaxLenght = 2000, TimeRespone = 3000, Format = "X"}
                //                        //}
                //                    }
                //                }
                //            }
                //        }
                //    }
                //},
                new ExchangeOption
                {
                    Id = 2,
                    Key = "TcpIp_table_46",
                    KeyTransport = new KeyTransport("TcpIp table 1", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ ДЛЯ ПЕРОНА 199
                                new RuleOption
                                {
                                    Name = "Peron_199",
                                    AddressDevice = "46",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    //DefaultItemJson= "{\"pathNumber\": \"5\"}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "46", //59
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%23",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "46", //59
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%39",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                new ExchangeOption
                {
                    Id = 3,
                    Key = "TcpIp_table_59",
                    KeyTransport = new KeyTransport("TcpIp table 1", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ ДЛЯ ПЕРОНА 199
                                new RuleOption
                                {
                                    Name = "Peron_199",
                                    AddressDevice = "59",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{\"pathNumber\": \"5\"}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //
                                        new ViewRuleOption
                                        {
                                            Id = 10,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "46", //59
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%23",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "46", //59
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%39",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                new ExchangeOption
                {
                    Id = 4,
                    Key = "TcpIp_table_NONE",
                    KeyTransport = new KeyTransport("TcpIp table 1", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ ДЛЯ ПЕРОНА 199
                                new RuleOption
                                {
                                    Name = "Peron_199",
                                    AddressDevice = "100",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{\"pathNumber\": \"5\"}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //
                                        new ViewRuleOption
                                        {
                                            Id = 10,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "46", //59
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%23",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "100", //59
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%39",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                //МНОГОСТРОЧКА
                new ExchangeOption
                {
                    Id = 5,
                    Key = "TcpIp_table_PribOtpr9Str",
                    KeyTransport = new KeyTransport("TcpIp table PribOtpr9Str", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ ДЛЯ МНОГОСТР. ТАБЛО ПРИБ/ОТПР
                                new RuleOption
                                {
                                    Name = "PribOtpr9Str_46",
                                    AddressDevice = "64",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 9, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%01000018{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{StationDeparture}-{StationArrival}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0101A07D{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0004000E%10{NumberOfCharacters:X2}01\\\"{StationDeparture}-{StationArrival}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0108009B{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0040001E%10{NumberOfCharacters:X2}01\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0109F0BC{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0040001E%10{NumberOfCharacters:X2}01\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010D50E3{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010E60FF{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0040001E%10{NumberOfCharacters:X2}01\\\"{DelayTime}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Platform}
                                        //new ViewRuleOption
                                        //{
                                        //    Id = 7,
                                        //    StartPosition = 0,
                                        //    Count = 9,
                                        //    BatchSize = 1,
                                        //    RequestOption = new RequestOption
                                        //    {
                                        //        Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                        //        Body = "%010C20D2{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{Platform}\\\"",
                                        //        Footer = "{CRCXor:X2}\u0003",
                                        //        MaxBodyLenght = 245,
                                        //        Format = "Windows-1251"
                                        //    },
                                        //    ResponseOption = new ResponseOption
                                        //    {
                                        //        Body = "0246463038254130373741434B454103",
                                        //        Lenght = 16,
                                        //        TimeRespone = 260,
                                        //        Format = "X2"
                                        //    }
                                        //}
                                    }
                                },
                                //СИНХР ВРЕМЕНИ
                                new RuleOption
                                {
                                    Name = "PribOtpr9Str_46_SyncTime",
                                    AddressDevice = "64",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "64",
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%23",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "64", 
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%39",
                                                Footer = "{CRCXor:X2}\u0003",
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },

                //TestPeronn_70_52_48-------------------------------------------------
                //ПЕРОННОЕ ТЕСТ 70
                new ExchangeOption
                {
                    Id = 6,
                    Key = "TcpIp_table_TestPeronn_70",
                    KeyTransport = new KeyTransport("TcpIp table TestPeronn_70_52_48", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                new RuleOption
                                {
                                    Name = "Peron_70",
                                    AddressDevice = "70",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                //ПЕРОННОЕ ТЕСТ 52
                new ExchangeOption
                {
                    Id = 7,
                    Key = "TcpIp_table_TestPeronn_52",
                    KeyTransport = new KeyTransport("TcpIp table TestPeronn_70_52_48", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                new RuleOption
                                {
                                    Name = "Peron_52",
                                    AddressDevice = "52",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                //ПЕРОННОЕ ТЕСТ 48
                new ExchangeOption
                {
                    Id = 8,
                    Key = "TcpIp_table_TestPeronn_48",
                    KeyTransport = new KeyTransport("TcpIp table TestPeronn_70_52_48", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                new RuleOption
                                {
                                    Name = "Peron_48",
                                    AddressDevice = "48",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },

                //TestPeronn_45_55_9-------------------------------------------------
                //ПЕРОННОЕ ТЕСТ 45
                new ExchangeOption
                {
                    Id = 9,
                    Key = "TcpIp_table_TestPeronn_45",
                    KeyTransport = new KeyTransport("TcpIp table TestPeronn_45_55_9", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                new RuleOption
                                {
                                    Name = "Peron_45",
                                    AddressDevice = "45",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                //ПЕРОННОЕ ТЕСТ 55
                new ExchangeOption
                {
                    Id = 10,
                    Key = "TcpIp_table_TestPeronn_55",
                    KeyTransport = new KeyTransport("TcpIp table TestPeronn_45_55_9", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                new RuleOption
                                {
                                    Name = "Peron_55",
                                    AddressDevice = "55",
                                    //WhereFilter = "(TypeTrain == \"Suburban\") && (PathNumber == \"2\" || PathNumber == \"3\" || PathNumber == \"4\")",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //Синхр. Времени
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 260,
                                                Format = "X2"
                                            }
                                        },
                                        //{Addition} {Stations}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{TypeAlias}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        },
                                        //{Note}
                                        new ViewRuleOption
                                        {
                                            Id = 9,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 600,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
                //ПЕРОННОЕ ТЕСТ 9 объединенный запрос
                new ExchangeOption
                {
                    Id = 12,
                    Key = "TcpIp_table_TestPeronn_9_Slim",
                    KeyTransport = new KeyTransport("TcpIp table TestPeronn_45_55_9", TransportType.TcpIp),
                    AutoStartCycleFunc = false, // DEBUG
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                new RuleOption
                                {
                                    Name = "Peron_9",
                                    AddressDevice = "9",
                                    WhereFilter = "(PathNumber == \"2\")",
                                    //WhereFilter = "true",
                                    OrderBy = "Id",
                                    //OrderBy = "ArrivalTime",
                                    TakeItems = 1, //2
                                    DefaultItemJson= "{}",  //"{}" - дефолтный конструктор типа
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //запрос 1
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"%0104F07500000f0040001E%10{NumberOfCharacters:X2}05\\\"{TArrival:t}\\\"%0107A0A000000f0040001E%10{NumberOfCharacters:X2}05\\\"{TDepart:t}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //запрос 2
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20DC00600f0020001E%10{NumberOfCharacters:X2}02\\\"{DelayTime:Min}\\\"%010DF0F000000f0040001E%10{NumberOfCharacters:X2}05\\\"{PathNumber}\\\"%010000f00210300004001E%10{NumberOfCharacters:X2}05\\\"{Note}\\\"%010A20C100600f0000001E%10{NumberOfCharacters:X2}02\\\"{TypeAlias}\\\"",
                                                Footer = "{CRCXor:X2}\u0003",
                                                MaxBodyLenght = 245,
                                                Format = "Windows-1251"
                                            },
                                            ResponseOption = new ResponseOption
                                            {
                                                Body = "0246463038254130373741434B454103",
                                                Lenght = 16,
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                },
            };

            await rep.AddRangeAsync(exchanges);
        }
    }
}

