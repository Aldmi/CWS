using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.Abstract.Entities.Options.MiddleWare.Hadlers;
using DAL.Abstract.Entities.Options.MiddleWare.Hadlers.StringConvertersOption;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;

namespace DeviceForExchnage.Benchmark.Datas
{
    public static class InDataSourse
    {

        public static MiddleWareInDataOption GetmiddleWareInDataOption()
        {
            MiddleWareInDataOption middleWareInDataOption = new MiddleWareInDataOption
            {
                Id = 1,
                Description = "Преобразование Note",
                StringHandlers = new List<StringHandlerMiddleWareOption>
                {
                    new StringHandlerMiddleWareOption
                    {
                        PropName = "Note",
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
                        PropName = "StationDeparture",
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



        public static InputData<AdInputType> GetData()
        {
           var datas= Enumerable.Range(0, 100).Select(i => new AdInputType
            {
                Id = i,
                Note = new Note
                {
                    NameRu = $"{i} Со всеми станциями кроие: Волочаевская, Климская",
                },
                StationDeparture = new Station
                {
                    NameRu = $"{i} Станция Отпр 1"
                }

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