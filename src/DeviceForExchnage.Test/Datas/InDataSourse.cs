using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DAL.Abstract.Entities.Options.MiddleWare.Handlers;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;

namespace DeviceForExchnage.Test.Datas
{
    public static class InDataSourse
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









        public static InputData<AdInputType> GetData(int countData)
        {
           var datas= Enumerable.Range(0, countData).Select(i => new AdInputType
            {
                Id = i,
                Note = new Note
                {
                    NameRu = $"Index= {i}   Со всеми станциями кроие: Волочаевская, Климская",
                },
                StationDeparture = new Station
                {
                    NameRu = $"Index= {i}    Станция Отпр 1"
                },
                NumberOfTrain = "956"
            }).ToList();


            var inData = new InputData<AdInputType>()
            {
                Command = Command4Device.None,
                DataAction = DataAction.CycleAction,
                DeviceName = "Peron.P1",
                DirectHandlerName = null,
                ExchangeName = "Exch Peron.P1",
                Data = datas
            };

            return inData;
        }


    }
}