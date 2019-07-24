using System;
using System.Linq;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;

namespace DeviceForExchnage.Test.Datas
{
    public static class InDataSourse
    {
        public static InputData<AdInputType> GetData(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType(
                i+1,
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
                    NameRu = $"Станция Отпр {i}"
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


        public static InputData<AdInputType> GetData_WithoutNote(int countData)
        {
            //var datas = Enumerable.Range(0, countData).Select(i => new AdInputType
            //{
            //    //Id = i,
            //    //StationDeparture = new Station
            //    //{
            //    //    NameRu = $"Index= {i}    Станция Отпр 1"
            //    //},
            //    //NumberOfTrain = "956"
            //}).ToList();

            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType(
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
                   NameRu = $"Index= {i}    Станция Отпр 1"
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


        public static InputData<AdInputType> GetData_NewNote(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType(
                i,
                "956",
                new Note
                {
                    NameRu = $"С остановками: Жуковская {i}, Римская {i}",
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


        public static InputData<AdInputType> GetData_Note_LongWord(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType(
                i,
                "956",
                new Note
                {
                    NameRu = $"Без остановок: Трофимово{i}, Воскресенск{i}, Шиферная{i}, Москворецкая{i}, Цемгигант{i}, Пески{i}, Золотая{i}, Конев Бор{i}, Хорошово{i}, Весенняя{i}, Сказочная{i}, Платформа 113 км{i}, Коломна{i}",
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


        public static InputData<AdInputType> GetData_StationCut(int countData)
        {
            var datas = Enumerable.Range(0, countData).Select(i => new AdInputType(
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
                    NameRu = $"Index= {i}  Станция Отпр 1"
                },
                new Station
                {
                    NameRu = $"Index= {i}  Станция Пртб 1"
                },
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