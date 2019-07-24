using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DAL.Abstract.Entities.Options.MiddleWare.Handlers;
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
            //var datas= Enumerable.Range(0, 100).Select(i => new AdInputType
            // {
            //     Id = i,
            //     Note = new Note
            //     {
            //         NameRu = $"{i} Со всеми станциями кроие: Волочаевская, Климская",
            //     },
            //     StationDeparture = new Station
            //     {
            //         NameRu = $"{i} Станция Отпр 1"
            //     }
            // }).ToList();


            var datas = Enumerable.Range(0, 100).Select(i => new AdInputType(
                i,
                "956",
                new Note
                {
                    NameRu = $"С остановками: Волочаевская {i}, Климская {i}, Октябрьская {i}, Новосибирская {i}, Красноярская {i}, 25 Километр {i}, Волховские холмы {i}, Ленинско кузнецкие золотые сопки верхней пыжмы {i}, Куйбышевская {i}, Казахстанская {i}, Свердлолвская {i}, Московская {i}, Горьковская {i}",
                },
                String.Empty,
                new EventTrain(0),
                null,
                new Station
                {
                    NameRu = $"Станция Отпр 1"
                },
                null,
                null,
                null
            )).ToList();
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