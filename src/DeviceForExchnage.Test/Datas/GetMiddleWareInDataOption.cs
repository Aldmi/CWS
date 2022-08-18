using System.Collections.Generic;
using Domain.Device.Enums;
using Domain.Device.MiddleWares4InData;
using Domain.Device.MiddleWares4InData.Handlers4InData;
using Shared.MiddleWares.ConvertersOption.EnumsConvertersOption;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.HandlersOption;

namespace DeviceForExchnage.Test.Datas
{
    public static class GetMiddleWareInDataOption
    {
        public static MiddleWareMediatorOption GetMiddleWareInDataOption_EnumHandlers_EnumerateConverter(string propName)
        {
            var middleWareInDataOption = new MiddleWareMediatorOption
            {
                Description = "перебор Lang",
                EnumHandlers = new List<EnumHandlerMiddleWare4InDataOption>
                {
                    new EnumHandlerMiddleWare4InDataOption
                    {
                        PropName = propName,
                        Path2Type = "Domain.InputDataModel.Autodictor.Entities.Lang, Domain.InputDataModel.Autodictor",
                        Converters = new List<UnitEnumConverterOption>
                        {
                            new UnitEnumConverterOption
                            {
                                EnumMemConverterOption = new EnumMemConverterOption
                                {
                                    DictChain = new Dictionary<string, int>
                                    {
                                        { "Ru", 1},
                                        { "Eng", 2}
                                    }
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareInDataOption;
        }


        public static MiddleWareMediatorOption GetMiddleWareInDataOption_LimitStringConverter(string propName)
        {
            var middleWareInDataOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption()
                    {
                        PropName = propName,
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                LimitStringConverterOption = new LimitStringConverterOption
                                {
                                    Limit = 80
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareInDataOption;
        }


        public static MiddleWareMediatorOption GetMiddleWareInDataOption_TwoStringHandlers(params string[] propNames)
        {
            MiddleWareMediatorOption middleWareMediatorOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propNames[0],
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                SubStringMemConverterOption = new SubStringMemConverterOption
                                {
                                    Lenght = 120,
                                    InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                                    Separator = ','
                                }
                            },
                            new UnitStringConverterOption
                            {
                                InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                                {
                                    LenghtLine = 40,
                                    Marker = "0x09"
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareMediatorOption;
        }


        public static MiddleWareMediatorOption GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter(string propName)
        {
            MiddleWareMediatorOption middleWareMediatorOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propName,
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                SubStringMemConverterOption = new SubStringMemConverterOption
                                {
                                    Lenght = 120,
                                    InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                                    Separator = ','
                                },
                            },
                            new UnitStringConverterOption
                            {
                                InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                                {
                                    LenghtLine = 40,
                                    Marker = "0x09"
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareMediatorOption;
        }



        public static MiddleWareMediatorOption GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_withoutPhrases(string propName)
        {
            MiddleWareMediatorOption middleWareMediatorOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propName,
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                SubStringMemConverterOption = new SubStringMemConverterOption
                                {
                                    Lenght = 120,
                                    InitPharases = null,
                                    Separator = ','
                                },
                            },
                            new UnitStringConverterOption
                            {
                                InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                                {
                                    LenghtLine = 40,
                                    Marker = "0x09"
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareMediatorOption;
        }



        public static MiddleWareMediatorOption GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_Khazanskiy(string propName)
        {
            MiddleWareMediatorOption middleWareMediatorOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propName,
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                SubStringMemConverterOption = new SubStringMemConverterOption
                                {
                                    Lenght = 90,
                                    InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                                    Separator = ','
                                }
                            },
                            new UnitStringConverterOption
                            {
                                InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                                {
                                    LenghtLine = 25,
                                    Marker = "0x09"
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareMediatorOption;
        }




        public static MiddleWareMediatorOption GetMiddleWareInDataOption_TwoStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter(params string[] propNames)
        {
            MiddleWareMediatorOption middleWareMediatorOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propNames[0],
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                SubStringMemConverterOption = new SubStringMemConverterOption
                                {
                                    Lenght = 100,
                                    InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                                    Separator = ','
                                }
                            },
                            new UnitStringConverterOption
                            {
                                InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                                {
                                    LenghtLine = 40,
                                    Marker = "0x09"
                                }
                            }
                        }
                    },
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propNames[1],
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                                {
                                    LenghtLine = 7,
                                    Marker = "0x09"
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareMediatorOption;
        }



        public static MiddleWareMediatorOption GetMiddleWareInDataOption_OneStringHandler_ReplaceEmptyStringConverterTest(params string[] propNames)
        {
            MiddleWareMediatorOption middleWareMediatorOption = new MiddleWareMediatorOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWare4InDataOption>
                {
                    new StringHandlerMiddleWare4InDataOption
                    {
                        PropName = propNames[0],
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                ReplaseStringConverterOption = new ReplaseStringConverterOption
                                {
                                    Mapping = new Dictionary<string, string>
                                    {
                                        {"", "ПОСАДКИ НЕТ" }
                                    }
                                }
                            }
                        }
                    }
                },
                InvokerOutput = new InvokerOutput
                {
                    Mode = InvokerOutputMode.Instantly
                }
            };

            return middleWareMediatorOption;
        }
    }
}