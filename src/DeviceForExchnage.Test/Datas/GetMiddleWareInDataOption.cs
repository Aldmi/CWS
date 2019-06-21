using System.Collections.Generic;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DAL.Abstract.Entities.Options.MiddleWare.Handlers;

namespace DeviceForExchnage.Test.Datas
{
    public static class GetMiddleWareInDataOption
    {
        public static MiddleWareInDataOption GetMiddleWareInDataOption_OneStringHandler(string propName)
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
                            Limit = 10
                        },
                        InseartStringConverterOption = new InseartStringConverterOption
                        {
                            InseartDict = new Dictionary<int, string>
                            {
                                {5, "0x09" },
                                {10, "0x09" },
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
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = propNames[0],
                        LimitStringConverterOption = new LimitStringConverterOption
                        {
                            Limit = 10
                        },
                        InseartStringConverterOption = new InseartStringConverterOption
                        {
                            InseartDict = new Dictionary<int, string>
                            {
                                {5, "0x09" },
                                {10, "0x09" },
                            }
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
                            Lenght = 100
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
                            Lenght = 100
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
                        SubStringMemConverterOption = new SubStringMemConverterOption
                        {
                            Priority = 1,
                            Lenght = 10
                        },
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