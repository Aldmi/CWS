using System;
using AutoMapper;
using Domain.InputDataModel.Autodictor.Model;
using FluentAssertions;
using WebApiSwc.AutoMapperConfig;
using WebApiSwc.DTO.XML;
using Xunit;

namespace MappingTest
{
    public class CreepingLine4XmlDtoMappingTest
    {
        [Fact]
        public void MappingFromDtoToObj()
        {
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            //Arrage
            var xmlDto = new CreepingLine4XmlDto()
            {
                Id = 0,
                Key = "c749ea14-9d9a-405c-aa2e-6f46c25f7d8c",
                Message = "уважаемые встречающие скоростной электропоезд Стрела сообщением Н.Новгород С.Петербург прибывает на 5 путь платформа номер 4 скоростной электропоезд Стрела сообщением ",
                StarTime = 1603780890073,
                Duration = 19052
            };

            var ts=  TimeSpan.FromMilliseconds(1603780890073);
            var dt= (new DateTime(1970, 1, 1)).AddMilliseconds(1603780890073);

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
    }
}