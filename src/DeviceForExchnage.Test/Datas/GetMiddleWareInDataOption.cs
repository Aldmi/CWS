using System.Collections.Generic;
using Domain.Device.Enums;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;
using Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption;

namespace DeviceForExchnage.Test.Datas
{
    public static class GetMiddleWareInDataOption
    {
        public static MiddleWareInDataOption GetMiddleWareInDataOption_LimitStringConverter(string propName)
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propName,
                        LimitStringConverterOption = new LimitStringConverterOption
                        {
                            Priority = 1,
                            Limit = 80
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propNames[0],
                        SubStringMemConverterOption = new SubStringMemConverterOption
                        {
                            Priority = 1,
                            Lenght = 120,
                            InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                            Separator = ','
                        },
                        InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                        {
                            Priority = 2,
                            LenghtLine = 40,
                            Marker = "0x09"
                        }
                    },
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propNames[1],
                        ReplaceEmptyStringConverterOption = new ReplaceEmptyStringConverterOption
                        {
                            ReplacementString = "Посадки нет"
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propName,
                        SubStringMemConverterOption = new SubStringMemConverterOption
                        {
                            Priority = 1,
                            Lenght = 120,
                            InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                            Separator = ','
                        },
                        InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                        {
                            Priority = 2,
                            LenghtLine = 40,
                            Marker = "0x09"
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propName,
                        SubStringMemConverterOption = new SubStringMemConverterOption
                        {
                            Priority = 1,
                            Lenght = 120,
                            InitPharases = null,
                            Separator = ','
                        },
                        InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                        {
                            Priority = 2,
                            LenghtLine = 40,
                            Marker = "0x09"
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propName,
                        SubStringMemConverterOption = new SubStringMemConverterOption
                        {
                            Priority = 1,
                            Lenght = 90,
                            InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                            Separator = ','
                        },
                        InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                        {
                            Priority = 2,
                            LenghtLine = 25,
                            Marker = "0x09"
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propNames[0],
                        SubStringMemConverterOption = new SubStringMemConverterOption
                        {
                            Priority = 1,
                            Lenght = 100,
                            InitPharases = new List<string> { "Без остановок: ", "С остановками: " },
                            Separator = ','
                        },
                        InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                        {
                            Priority = 2,
                            LenghtLine = 40,
                            Marker = "0x09"
                        }
                    },
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propNames[1],
                        InseartEndLineMarkerConverterOption = new InseartEndLineMarkerConverterOption
                        {
                            Priority = 1,
                            LenghtLine = 7,
                            Marker = "0x09"
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propNames[0],
                        ReplaceEmptyStringConverterOption = new ReplaceEmptyStringConverterOption
                        {
                            Priority = 1,
                            ReplacementString = "ПОСАДКИ НЕТ"
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