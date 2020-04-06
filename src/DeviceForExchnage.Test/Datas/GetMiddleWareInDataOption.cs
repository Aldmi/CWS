using System.Collections.Generic;
using Domain.Device.Enums;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;
using Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption;

namespace DeviceForExchnage.Test.Datas
{
    public static class GetMiddleWareInDataOption
    {
        public static MiddleWareInDataOption GetMiddleWareInDataOption_EnumHandlers_EnumerateConverter(string propName)
        {
            var middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "перебор Lang",
                EnumHandlers = new List<EnumHandlerMiddleWareOption>
                {
                    new EnumHandlerMiddleWareOption
                    {
                        PropName = propName,
                        EnumMemConverterOption = new EnumMemConverterOption
                        {
                            //Priority = 1,
                            DictChain = new Dictionary<string, int>
                            {
                                { "Ru", 1},
                                { "Eng", 2}
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


        public static MiddleWareInDataOption GetMiddleWareInDataOption_LimitStringConverter(string propName)
        {
            var middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption()
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


        public static MiddleWareInDataOption GetMiddleWareInDataOption_TwoStringHandlers(params string[] propNames)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption
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
                    },
                    new StringMiddleWareOption
                    {
                        PropName = propNames[1],
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                ReplaceEmptyStringConverterOption = new ReplaceEmptyStringConverterOption
                                {
                                    ReplacementString = "Посадки нет"
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


        public static MiddleWareInDataOption GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter(string propName)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption
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

            return middleWareInDataOption;
        }



        public static MiddleWareInDataOption GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_withoutPhrases(string propName)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption
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

            return middleWareInDataOption;
        }



        public static MiddleWareInDataOption GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_Khazanskiy(string propName)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption
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

            return middleWareInDataOption;
        }




        public static MiddleWareInDataOption GetMiddleWareInDataOption_TwoStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter(params string[] propNames)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption
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
                    new StringMiddleWareOption
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

            return middleWareInDataOption;
        }



        public static MiddleWareInDataOption GetMiddleWareInDataOption_OneStringHandler_ReplaceEmptyStringConverterTest(params string[] propNames)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringMiddleWareOption>
                {
                    new StringMiddleWareOption
                    {
                        PropName = propNames[0],
                        Converters = new List<UnitStringConverterOption>
                        {
                            new UnitStringConverterOption
                            {
                                ReplaceEmptyStringConverterOption = new ReplaceEmptyStringConverterOption
                                {
                                    ReplacementString = "ПОСАДКИ НЕТ"
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
    }
}