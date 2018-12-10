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
                #region Plat2.P10

                new ExchangeOption
                {
                    Id = 1,
                    Key = "TcpIp=200 Plat=2 P=10 Stolb=10 Addr=2",
                    KeyTransport = new KeyTransport("TcpIp=200", TransportType.TcpIp),
                   AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "2",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "2",
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
                                    AddressDevice = "2",
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
                    Id = 2,
                    Key = "TcpIp=200 Plat=2 P=10 Stolb=10 Addr=1",
                    KeyTransport = new KeyTransport("TcpIp=200", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "1",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "1",
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
                                    AddressDevice = "1",
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
                    Key = "TcpIp=201 Plat=2 P=10 Stolb=9 Addr=5",
                    KeyTransport = new KeyTransport("TcpIp=201", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "5",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "5",
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
                                    AddressDevice = "5",
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
                    Key = "TcpIp=201 Plat=2 P=10 Stolb=9 Addr=14",
                    KeyTransport = new KeyTransport("TcpIp=201", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "14",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "14",
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
                                    AddressDevice = "14",
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
                    Id = 5,
                    Key = "TcpIp=209 Plat=2 P=10 Stolb=_ Addr=9",
                    KeyTransport = new KeyTransport("TcpIp=209", TransportType.TcpIp),
                   AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "9",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                                TimeRespone = 1000,
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
                                                TimeRespone = 1000,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "9",
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
                                    AddressDevice = "9",
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
                    Id = 6,
                    Key = "TcpIp=210 Plat=2 P=10 Stolb=_ Addr=70",
                    KeyTransport = new KeyTransport("TcpIp=210", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "70",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "70",
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
                                    AddressDevice = "70",
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

                #endregion



                #region (Plat2&Plat1).P12

                new ExchangeOption
                {
                    Id = 50,
                    Key = "TcpIp=200 Plat=2 P=12 Stolb=10 Addr=25",
                    KeyTransport = new KeyTransport("TcpIp=200", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "25",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "25",
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
                                    AddressDevice = "25",
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
                    Id = 51,
                    Key = "TcpIp=200 Plat=2 P=12 Stolb=10 Addr=21",
                    KeyTransport = new KeyTransport("TcpIp=200", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "21",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "21",
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
                                    AddressDevice = "21",
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
                    Id = 52,
                    Key = "TcpIp=201 Plat=2 P=12 Stolb=9 Addr=6",
                    KeyTransport = new KeyTransport("TcpIp=201", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "6",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "6",
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
                                    AddressDevice = "6",
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
                    Id = 53,
                    Key = "TcpIp=201 Plat=2 P=12 Stolb=9 Addr=7",
                    KeyTransport = new KeyTransport("TcpIp=201", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "7",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "7",
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
                                    AddressDevice = "7",
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
                    Id = 54,
                    Key = "TcpIp=196 Plat=1 P=12 Stolb=_ Addr=71",
                    KeyTransport = new KeyTransport("TcpIp=196", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "71",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "71",
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
                                    AddressDevice = "71",
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
                    Id = 55,
                    Key = "TcpIp=196 Plat=1 P=12 Stolb=_ Addr=72",
                    KeyTransport = new KeyTransport("TcpIp=196", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "72",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "72",
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
                                    AddressDevice = "72",
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
                    Id = 56,
                    Key = "TcpIp=197 Plat=1 P=12 Stolb=_ Addr=47",
                    KeyTransport = new KeyTransport("TcpIp=197", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "47",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "47",
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
                                    AddressDevice = "47",
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
                    Id = 57,
                    Key = "TcpIp=197 Plat=1 P=12 Stolb=_ Addr=60",
                    KeyTransport = new KeyTransport("TcpIp=197", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "60",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "60",
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
                                    AddressDevice = "60",
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
                    Id = 58,
                    Key = "TcpIp=198 Plat=1 P=12 Stolb=_ Addr=57",
                    KeyTransport = new KeyTransport("TcpIp=198", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "57",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "57",
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
                                    AddressDevice = "57",
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
                    Id = 59,
                    Key = "TcpIp=198 Plat=1 P=12 Stolb=_ Addr=58",
                    KeyTransport = new KeyTransport("TcpIp=198", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "58",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "58",
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
                                    AddressDevice = "58",
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
                    Id = 60,
                    Key = "TcpIp=199 Plat=1 P=12 Stolb=_ Addr=46",
                    KeyTransport = new KeyTransport("TcpIp=199", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "46",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "46",
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
                                    AddressDevice = "46",
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
                    Id = 61,
                    Key = "TcpIp=199 Plat=1 P=12 Stolb=_ Addr=59",
                    KeyTransport = new KeyTransport("TcpIp=199", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "59",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "59",
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
                                    AddressDevice = "59",
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
                    Id = 62,
                    Key = "TcpIp=209 Plat=2 P=12 Stolb=_ Addr=9",
                    KeyTransport = new KeyTransport("TcpIp=209", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "9",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "9",
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
                                    AddressDevice = "9",
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
                    Id = 63,
                    Key = "TcpIp=210 Plat=2 P=12 Stolb=_ Addr=70",
                    KeyTransport = new KeyTransport("TcpIp=210", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "70",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "70",
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
                                    AddressDevice = "70",
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

                #endregion



                #region Plat3.P6

                new ExchangeOption
                {
                    Id = 100,
                    Key = "TcpIp=203 Plat=3 P=6 Stolb=7 Addr=49",
                    KeyTransport = new KeyTransport("TcpIp=203", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "49",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "49",
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
                                    AddressDevice = "49",
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
                    Id = 101,
                    Key = "TcpIp=203 Plat=3 P=6 Stolb=7 Addr=51",
                    KeyTransport = new KeyTransport("TcpIp=203", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "51",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "51",
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
                                    AddressDevice = "51",
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
                    Id = 102,
                    Key = "TcpIp=204 Plat=3 P=6 Stolb=6 Addr=17",
                    KeyTransport = new KeyTransport("TcpIp=204", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "17",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "17",
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
                                    AddressDevice = "17",
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
                    Id = 103,
                    Key = "TcpIp=204 Plat=3 P=6 Stolb=6 Addr=19",
                    KeyTransport = new KeyTransport("TcpIp=204", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "19",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "19",
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
                                    AddressDevice = "19",
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
                    Id = 104,
                    Key = "TcpIp=205 Plat=3 P=6 Stolb=5 Addr=61",
                    KeyTransport = new KeyTransport("TcpIp=205", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "61",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "61",
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
                                    AddressDevice = "61",
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
                    Id = 105,
                    Key = "TcpIp=205 Plat=3 P=6 Stolb=5 Addr=62",
                    KeyTransport = new KeyTransport("TcpIp=205", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "62",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "62",
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
                                    AddressDevice = "62",
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
                    Id = 106,
                    Key = "TcpIp=209 Plat=3 P=6 Stolb=_ Addr=55",
                    KeyTransport = new KeyTransport("TcpIp=209", TransportType.TcpIp),
                   AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "55",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "55",
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
                                    AddressDevice = "55",
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
                    Id = 28,
                    Key = "TcpIp=210 Plat=3 P=6 Stolb=_ Addr=52",
                    KeyTransport = new KeyTransport("TcpIp=210", TransportType.TcpIp),
                   AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "52",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "52",
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
                                    AddressDevice = "52",
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

                #endregion



                #region Plat3.P8

                new ExchangeOption
                {
                    Id = 150,
                    Key = "TcpIp=203 Plat=3 P=8 Stolb=7 Addr=50",
                    KeyTransport = new KeyTransport("TcpIp=203", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "50",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "50",
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
                                    AddressDevice = "50",
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
                    Id = 151,
                    Key = "TcpIp=203 Plat=3 P=8 Stolb=7 Addr=53",
                    KeyTransport = new KeyTransport("TcpIp=203", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "53",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "53",
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
                                    AddressDevice = "53",
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
                    Id = 152,
                    Key = "TcpIp=204 Plat=3 P=8 Stolb=6 Addr=54",
                    KeyTransport = new KeyTransport("TcpIp=204", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "54",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "54",
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
                                    AddressDevice = "54",
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
                    Id = 153,
                    Key = "TcpIp=204 Plat=3 P=8 Stolb=6 Addr=56",
                    KeyTransport = new KeyTransport("TcpIp=204", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "56",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "56",
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
                                    AddressDevice = "56",
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
                    Id = 154,
                    Key = "TcpIp=205 Plat=3 P=8 Stolb=5 Addr=63",
                    KeyTransport = new KeyTransport("TcpIp=205", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "63",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "63",
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
                                    AddressDevice = "63",
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
                    Id = 155,
                    Key = "TcpIp=205 Plat=3 P=8 Stolb=5 Addr=73",
                    KeyTransport = new KeyTransport("TcpIp=205", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "73",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "73",
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
                                    AddressDevice = "73",
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
                    Id = 156,
                    Key = "TcpIp=209 Plat=3 P=8 Stolb=_ Addr=55",
                    KeyTransport = new KeyTransport("TcpIp=209", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "55",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "55",
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
                                    AddressDevice = "55",
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
                    Id = 157,
                    Key = "TcpIp=210 Plat=3 P=8 Stolb=_ Addr=52",
                    KeyTransport = new KeyTransport("TcpIp=210", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "52",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "52",
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
                                    AddressDevice = "52",
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

                #endregion



                #region Plat4.P5

                new ExchangeOption
                {
                    Id = 200,
                    Key = "TcpIp=206 Plat=4 P=5 Stolb=4 Addr=8",
                    KeyTransport = new KeyTransport("TcpIp=206", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "8",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "8",
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
                                    AddressDevice = "8",
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
                    Id = 201,
                    Key = "TcpIp=206 Plat=4 P=5 Stolb=4 Addr=11",
                    KeyTransport = new KeyTransport("TcpIp=206", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "11",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "11",
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
                                    AddressDevice = "11",
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
                    Id = 202,
                    Key = "TcpIp=207 Plat=4 P=5 Stolb=3 Addr=4",
                    KeyTransport = new KeyTransport("TcpIp=207", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "4",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "4",
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
                                    AddressDevice = "4",
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
                    Id = 203,
                    Key = "TcpIp=207 Plat=4 P=5 Stolb=3 Addr=13",
                    KeyTransport = new KeyTransport("TcpIp=207", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "13",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "13",
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
                                    AddressDevice = "13",
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
                    Id = 204,
                    Key = "TcpIp=208 Plat=4 P=5 Stolb=2 Addr=10",
                    KeyTransport = new KeyTransport("TcpIp=208", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "10",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "10",
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
                                    AddressDevice = "10",
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
                    Id = 205,
                    Key = "TcpIp=208 Plat=4 P=5 Stolb=2 Addr=18",
                    KeyTransport = new KeyTransport("TcpIp=208", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "18",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "18",
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
                                    AddressDevice = "18",
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
                    Id = 206,
                    Key = "TcpIp=209 Plat=4 P=5 Stolb=_ Addr=45",
                    KeyTransport = new KeyTransport("TcpIp=209", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "45",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "45",
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
                                    AddressDevice = "45",
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
                    Id = 207,
                    Key = "TcpIp=210 Plat=4 P=5 Stolb=_ Addr=48",
                    KeyTransport = new KeyTransport("TcpIp=210", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "48",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "48",
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
                                    AddressDevice = "48",
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
                #endregion



                #region Plat4.P3

                new ExchangeOption
                {
                    Id = 250,
                    Key = "TcpIp=206 Plat=4 P=3 Stolb=4 Addr=26",
                    KeyTransport = new KeyTransport("TcpIp=206", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "26",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "26",
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
                                    AddressDevice = "26",
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
                    Id = 251,
                    Key = "TcpIp=206 Plat=4 P=3 Stolb=4 Addr=44",
                    KeyTransport = new KeyTransport("TcpIp=206", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "44",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "44",
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
                                    AddressDevice = "44",
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
                    Id = 252,
                    Key = "TcpIp=207 Plat=4 P=3 Stolb=3 Addr=23",
                    KeyTransport = new KeyTransport("TcpIp=207", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "23",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "23",
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
                                    AddressDevice = "23",
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
                    Id = 253,
                    Key = "TcpIp=207 Plat=4 P=3 Stolb=3 Addr=29",
                    KeyTransport = new KeyTransport("TcpIp=207", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "29",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "29",
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
                                    AddressDevice = "29",
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
                    Id = 254,
                    Key = "TcpIp=208 Plat=4 P=3 Stolb=2 Addr=3",
                    KeyTransport = new KeyTransport("TcpIp=208", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "3",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "3",
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
                                    AddressDevice = "3",
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
                    Id = 255,
                    Key = "TcpIp=208 Plat=4 P=3 Stolb=2 Addr=12",
                    KeyTransport = new KeyTransport("TcpIp=208", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "12",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "12",
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
                                    AddressDevice = "12",
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
                    Id = 256,
                    Key = "TcpIp=209 Plat=4 P=3 Stolb=_ Addr=45",
                    KeyTransport = new KeyTransport("TcpIp=209", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "45",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "45",
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
                                    AddressDevice = "45",
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
                    Id = 257,
                    Key = "TcpIp=210 Plat=4 P=3 Stolb=_ Addr=48",
                    KeyTransport = new KeyTransport("TcpIp=210", TransportType.TcpIp),
                  AutoStartCycleFunc = false,
                    NumberErrorTrying = 30,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "48",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 1,
                                    DefaultItemJson= "{}",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "48",
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
                                    AddressDevice = "48",
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
                #endregion




                #region PribOtpr_9Str_4Table

                new ExchangeOption
                {
                    Id = 300,
                    Key = "TcpIp=194 Event=PribOtpr NItem=9 Addr=64",
                    KeyTransport = new KeyTransport("TcpIp=194", TransportType.TcpIp),
                  AutoStartCycleFunc = true,
                    NumberErrorTrying = 60,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "64",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 9,
                                    DefaultItemJson= "{}",
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{StationDeparture}-{StationArrival}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{Platform}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20D2{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{Platform}\\\"",
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
                                        //{SyncTInSec}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010D60FF07307F0000001E%110406NNNNN",
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
                                                TimeRespone = 800,
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
                new ExchangeOption
                {
                    Id = 301,
                    Key = "TcpIp=195 Event=PribOtpr NItem=9 Addr=65",
                    KeyTransport = new KeyTransport("TcpIp=195", TransportType.TcpIp),
                  AutoStartCycleFunc = true,
                    NumberErrorTrying = 60,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "65",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 9,
                                    DefaultItemJson= "{}",
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{StationDeparture}-{StationArrival}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{Platform}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20D2{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{Platform}\\\"",
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
                                        //{SyncTInSec}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010D60FF07307F0000001E%110406NNNNN",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "65",
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "65",
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
                new ExchangeOption
                {
                    Id = 302,
                    Key = "TcpIp=195 Event=PribOtpr NItem=9 Addr=66",
                    KeyTransport = new KeyTransport("TcpIp=195", TransportType.TcpIp),
                  AutoStartCycleFunc = true,
                    NumberErrorTrying = 60,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "66",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 9,
                                    DefaultItemJson= "{}",
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{StationDeparture}-{StationArrival}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{Platform}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20D2{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{Platform}\\\"",
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
                                        //{SyncTInSec}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010D60FF07307F0000001E%110406NNNNN",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "66",
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "66",
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
                new ExchangeOption
                {
                    Id = 303,
                    Key = "TcpIp=195 Event=PribOtpr NItem=9 Addr=67",
                    KeyTransport = new KeyTransport("TcpIp=195", TransportType.TcpIp),
                  AutoStartCycleFunc = true,
                    NumberErrorTrying = 60,
                    NumberTimeoutTrying = 5,
                    Provider = new ProviderOption
                    {
                        Name = "ByRules",
                        ByRulesProviderOption = new ByRulesProviderOption
                        {
                            Rules = new List<RuleOption>
                            {
                                //ДАННЫЕ
                                new RuleOption
                                {
                                    Name = "Data",
                                    AddressDevice = "67",
                                    WhereFilter = "true",
                                    OrderBy = "Id",
                                    TakeItems = 9,
                                    DefaultItemJson= "{}",
                                    ViewRules = new List<ViewRuleOption>
                                    {
                                        //{NumberOfTrain}
                                        new ViewRuleOption
                                        {
                                            Id = 1,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{StationDeparture}-{StationArrival}
                                        new ViewRuleOption
                                        {
                                            Id = 2,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TArrival:t}
                                        new ViewRuleOption
                                        {
                                            Id = 3,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{TDepart:t}
                                        new ViewRuleOption
                                        {
                                            Id = 4,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{PathNumber}
                                        new ViewRuleOption
                                        {
                                            Id = 5,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{DelayTime}
                                        new ViewRuleOption
                                        {
                                            Id = 6,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        },
                                        //{Platform}
                                        new ViewRuleOption
                                        {
                                            Id = 7,
                                            StartPosition = 0,
                                            Count = 9,
                                            BatchSize = 3,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%010C20D2{(rowNumber*11-11):X3}{(rowNumber*11-2):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{Platform}\\\"",
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
                                        //{SyncTInSec}
                                        new ViewRuleOption
                                        {
                                            Id = 8,
                                            StartPosition = 0,
                                            Count = 1,
                                            BatchSize = 1000,
                                            RequestOption = new RequestOption
                                            {
                                                Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                                                Body = "%30{SyncTInSec:X5}%010D60FF07307F0000001E%110406NNNNN",
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
                                //КОМАНДА ОЧИСТКИ
                                new RuleOption
                                {
                                    Name = "Command_Clear",
                                    AddressDevice = "67",
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
                                                TimeRespone = 800,
                                                Format = "X2"
                                            }
                                        }
                                    }
                                },
                                //КОМАНДА ПЕРЕЗАГРУЗКИ
                                new RuleOption
                                {
                                    Name = "Command_Restart",
                                    AddressDevice = "67",
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

                #endregion
            };

            await rep.AddRangeAsync(exchanges);
        }
    }
}

