using AutoMapper;
using FluentAssertions;
using InputDataModel.Autodictor.Model;
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
            //res.Should().BeEmpty();

        }

    }
}