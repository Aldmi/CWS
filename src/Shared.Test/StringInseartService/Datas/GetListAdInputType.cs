using System;
using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Newtonsoft.Json;

namespace Shared.Test.StringInseartService.Datas
{
    public static class GetListAdInputType
    {
        public static List<AdInputType> ListInputType1Item => new List<AdInputType>
        {
            new AdInputType(1, 1, 1, Lang.Ru, 
                "456",
                "5",
                null,
                new EventTrain(1),
                new TypeTrain("Скорый", "СКОР",String.Empty,String.Empty, 1),
                new VagonDirection(), 
                new Station {NameRu = "Питер"},
                new Station {NameRu = "Москва"},
                null,
                null,
                null,
                DateTime.Parse("15:25"),
                DateTime.Parse("16:18"),
                DateTime.Parse("00:15"),
                DateTime.Parse("15:40"),
                null,
                new Addition{NameRu = String.Empty},
                new Note {NameRu = "Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7"},
                null,
                null,
                null,
                null,
                null)
        };




        public static List<AdInputType> ListInputTypesDefault => new List<AdInputType>
        {
            new AdInputType(1, 1, 1, Lang.Ru, 
                "456",
                "5",
                null,
                new EventTrain(1),
                new TypeTrain("Скорый", "СКОР",String.Empty,String.Empty, 1),
                new VagonDirection(), 
                new Station {NameRu = "Питер"},
                new Station {NameRu = "Москва"},
                null,
                null,
                null,
                DateTime.Parse("15:25"),
                DateTime.Parse("16:18"),
                DateTime.Parse("00:15"),
                DateTime.Parse("15:40"),
                null,
                new Addition{NameRu = String.Empty},
                new Note {NameRu = "Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7"},
                null,
                null,
                null,
                null,
                null),

            new AdInputType(2, 1, 1, Lang.Ru, 
                "563",
                "5",
                null,
                new EventTrain(1),
                new TypeTrain("Скорый", "СКОР",String.Empty,String.Empty, 1),
                new VagonDirection(), 
                new Station {NameRu = "Лондон"},
                new Station {NameRu = "Питер"},
                null,
                null,
                null,
                DateTime.Parse("15:19"),
                DateTime.Parse("17:18"),
                DateTime.Parse("00:15"),
                DateTime.Parse("15:40"),
                null,
                new Addition{NameRu = String.Empty},
                new Note {NameRu = "Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7"},
                null,
                null,
                null,
                null,
                null),

            new AdInputType(3, 1, 1, Lang.Ru, 
                "563",
                "5",
                null,
                new EventTrain(0),
                new TypeTrain("Скорый", "СКОР",String.Empty,String.Empty, 1),
                new VagonDirection(), 
                new Station {NameRu = "Новосибирск"},
                new Station {NameRu = "Питер"},
                null,
                null,
                null,
                DateTime.Parse("11:22"),
                DateTime.Parse("20:20"),
                DateTime.Parse("00:15"),
                DateTime.Parse("15:40"),
                null,
                new Addition{NameRu = String.Empty},
                new Note {NameRu = "Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7"},
                null,
                null,
                null,
                null,
                null)
        };


        public static List<AdInputType> GenerateListInputType(int count, int mult)
        {
            var list = Enumerable
                .Range(0, count)
                .Select(item => new AdInputType(item, item, item, Lang.Ru,
                    (item * mult).ToString(),
                    "5",
                    null,
                    new EventTrain(0),
                    new TypeTrain("Скорый", "СКОР", String.Empty, String.Empty, 1),
                    new VagonDirection(),
                    new Station {NameRu = "Новосибирск"},
                    new Station {NameRu = "Питер"},
                    null,
                    null,
                    null,
                    DateTime.Parse("11:22"),
                    DateTime.Parse("20:20"),
                    DateTime.Parse("00:15"),
                    DateTime.Parse("15:40"),
                    null,
                    new Addition {NameRu = String.Empty},
                    new Note {NameRu = "Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7"},
                    null,
                    null,
                    null,
                    null,
                    null)).ToList();

            return list;
        }


        public static List<AdInputType> GenerateDefaultListInputType(int count)
        {
            var list = Enumerable
                .Range(0, count)
                .Select(item => JsonConvert.DeserializeObject<AdInputType>("{}")).ToList();

            return list;
        }


    }
}