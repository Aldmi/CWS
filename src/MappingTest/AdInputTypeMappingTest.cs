using System;
using System.Collections.Generic;
using AutoMapper;
using Domain.InputDataModel.Autodictor.Model;
using FluentAssertions;
using Shared.Helpers;
using WebApiSwc.AutoMapperConfig;
using WebApiSwc.DTO.XML;
using Xunit;

namespace MappingTest
{
    public class AdInputTypeMappingTest
    {
        [Fact]
        public void MappingFromAdInputType4XmlDto()
        {
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            //Arrage
            var xmlDto = new AdInputType4XmlDto()
            {
                Note = "Со всеми: Московский, Волжский",
                ScheduleId = "254688",
                TrainNumber = "740",
                Addition = "Дополнение 111",
                DaysOfGoing = "Без выходных",
                Direction = "1",
                EmergencySituation = "0",

                RecDateTime = "2019-07-09T08:14:00",
                SndDateTime = "2019-07-09T08:56:00",

                DirectionStation = "Крымское",
                StartStation = "Москва",
                EndStation = "Кисловодск",
                StartStationENG = "Moscow",
                EndStationENG = "Kislovodsk",

                TrackNumber = "5",

                TypeName = "Скоростной",
                TrainType ="1",

                HereDateTime = "00:14:00",
                //ExpectedTime = "00:22:00",
            };

            //Act
            var res = mapper.Map<AdInputType>(xmlDto);

            //Asert
            res.Note.NameRu.Should().Be("Со всеми: Московский, Волжский");
            res.ScheduleId.Should().Be(254688);
            res.NumberOfTrain.Should().Be("740");
            res.Addition.NameRu.Should().Be("Дополнение 111");
            res.DaysFollowing.NameRu.Should().Be("Без выходных");

            res.DirectionStation.NameRu.Should().Be("Крымское");
            res.ArrivalTime.Should().Be(DateTime.Parse("2019-07-09T08:14:00"));
            res.DepartureTime.Should().Be(DateTime.Parse("2019-07-09T08:56:00"));

            res.StationArrival.NameRu.Should().Be("Кисловодск");
            res.StationDeparture.NameRu.Should().Be("Москва");
            res.StationArrival.NameEng.Should().Be("Kislovodsk");
            res.StationDeparture.NameEng.Should().Be("Moscow");

            res.StationsCut.NameRu.Should().Be("Кисловодск");
            res.Stations.NameRu.Should().Be("Москва-Кисловодск");

            res.PathNumber.Should().Be("5");
            res.Event.NameRu.Should().Be("Отправление");

            res.StopTime.Should().Be(TimeSpan.Parse("00:14:00"));
        }



        [Fact]
        public void tttt()
        {
            string str = "aaa ";

            // int count = 7;
            // int batchSize = 3;

            //int count = 1;
            // int batchSize = 1;

            int count = 10;
            int batchSize = 2;

            //int count = 3;
            //int batchSize = 3;

            var res = HelperString.CalcBatchedSequence(str, count, batchSize);
        }
    }
}