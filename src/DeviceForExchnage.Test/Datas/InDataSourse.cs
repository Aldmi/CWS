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
        public static InputData<AdInputType> GetData_Note(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType
            {
                Id = i,
                Note = new Note
                {
                    NameRu = $"С остановками: Волочаевская {i}, Климская {i}, Октябрьская {i}, Новосибирская {i}, Красноярская {i}, 25 Километр {i}, Волховские холмы {i}, Ленинско кузнецкие золотые сопки верхней пыжмы {i}, Куйбышевская {i}, Казахстанская {i}, Свердлолвская {i}, Московская {i}, Горьковская {i}",
                },
                StationDeparture = new Station
                {
                    NameRu = $"Станция Отпр 1"
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


        public static InputData<AdInputType> GetData_WithoutNote(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType
            {
                Id = i,
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


        public static InputData<AdInputType> GetData_NewNote(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType
            {
                Id = i,
                Note = new Note
                {
                    NameRu = $"Index= {i} Поезд Следует без остановок",
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


        public static InputData<AdInputType> GetData_Note_LongWord(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType
            {
                Id = i,
                Note = new Note
                {
                    NameRu = $"Без остановок: Трофимово{i}, Воскресенск{i}, Шиферная{i}, Москворецкая{i}, Цемгигант{i}, Пески{i}, Золотая{i}, Конев Бор{i}, Хорошово{i}, Весенняя{i}, Сказочная{i}, Платформа 113 км{i}, Коломна{i}",
                },
                StationDeparture = new Station
                {
                    NameRu = $"Станция Отпр {i}"
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