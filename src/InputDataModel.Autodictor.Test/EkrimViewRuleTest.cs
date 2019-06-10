using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using FluentAssertions;
using InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using Moq;
using Serilog;
using Xunit;

namespace InputDataModel.Autodictor.Test
{
    public class EkrimViewRuleTest
    {

        private readonly ILogger _logger;



        #region ctor

        public EkrimViewRuleTest()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(loger => loger.Debug(""));
            mock.Setup(loger => loger.Error(""));
            mock.Setup(loger => loger.Warning(""));
            _logger = mock.Object;

        }

        #endregion



        #region SharedData4Tests

        //List<AdInputType> inTypes, RequestOption requestOption, string formatExcpected, string requestStringExcpected
        public static IEnumerable<object[]> GetDataRequestStringTestInputDatas => new[]
        {
            new object[]
            {
                new List<AdInputType>
                {
                    new AdInputType
                    {
                        Id = 1,
                        Lang = Lang.Ru,
                        NumberOfTrain = "2",
                        StationArrival = new Station{NameRu = "КАЗАНЬ"},
                        ArrivalTime = DateTime.Parse("20:50"),
                    }
                },
                new RequestOption
                {
                    Format = "Windows-1251",
                    MaxBodyLenght = 245,
                    Header = "0xFF0xFF0x020x1B0x57",
                    Body = "{StationArrival}0x09{NumberOfTrain}0x09{TArrival:t}0x09",
                    Footer = "0x030x{CRCXor[0x02-0x03]:X2}0x1F"
                },
                1,     //count
                0,     //start
                1000,  //batch
                new List<string>
                {
                    "HEX",
                    "HEX"
                },
                new List<string>
                {
                    "FFFF021B57CAC0C7C0CDDC09320932303A35300903561F"
                }        
            },
            new object[]
            {
                new List<AdInputType>
                {
                    new AdInputType
                    {
                        Id = 1,
                        Lang = Lang.Ru,
                        NumberOfTrain = "90",
                        StationArrival = new Station{NameRu = "КАЗАНЬ"},
                        DepartureTime = DateTime.Parse("19:20"),
                        TrainType = new TypeTrain{NameAliasRu = "скор"},
                        Event = new EventTrain(1)
                    },
                    new AdInputType
                    {
                        Id = 1,
                        Lang = Lang.Ru,
                        NumberOfTrain = "90",
                        StationArrival = new Station{NameRu = "КАЗАНЬ"},
                        DepartureTime = DateTime.Parse("19:20"),
                        TrainType = new TypeTrain{NameAliasRu = "скор"},
                        Event = new EventTrain(1)
                    }
                },
                new RequestOption
                {
                    Format = "Windows-1251",
                    MaxBodyLenght = 245,
                    Header = "0xFF0xFF0x020x1B0x57",
                    Body = "0x{(rowNumber+64):X1}0x46{NumberOfTrain}0x09{TypeAlias}0x09{StationsCut}0x09{TDepart:t}0x09{PathNumber}0x09",
                    Footer = "0x030x{CRCXor[0x02-0x03]:X2}0x1F"
                },
                2,     //count
                0,     //start
                1,     //batch
                new List<string>
                {
                    "HEX",
                    "HEX"
                },
                new List<string>
                {
                    "FFFF021B574146393009F1EAEEF009CAC0C7C0CDDC0931393A323009200903421F",
                    "FFFF021B574246393009F1EAEEF009CAC0C7C0CDDC0931393A323009200903411F"
                }     
            }
        };

        #endregion




        [Theory]
        [MemberData(nameof(GetDataRequestStringTestInputDatas))]
        public void GetDataRequestStringTest(
            List<AdInputType> inTypes,
            RequestOption requestOption,
            int count,
            int startPosition,
            int batchSize,
            List<string> formatsExcpected,
            List<string> requestsStringExcpected)
        {
            // Arrange
            var address = "9";
            var option = new ViewRuleOption()
            {
                Id = 1,
                Count = count,
                StartPosition = startPosition,
                BatchSize = batchSize,
                RequestOption = requestOption,
                ResponseOption = null
            };
            var viewRule = new ViewRule(address, option, _logger);

            //Act
            var requests = viewRule.GetDataRequestString(inTypes).ToList();
            for (int i = 0; i < requests.Count; i++)
            {
                var request = requests[i];
                var requestStringExcpected= requestsStringExcpected[i];
                var formatExcpected = formatsExcpected[i];

                var requestString = request?.Request.StrRepresent.Str;
                var currentFormat = request?.Request.StrRepresent.Format;

                //Asssert
                request.Should().NotBeNull();
                requestString.Should().NotBeNull();
                requestString.Should().Contain(requestStringExcpected);
                currentFormat.Should().Be(formatExcpected);
            }
        }
    }
}