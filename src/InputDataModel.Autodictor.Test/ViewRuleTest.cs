using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using FluentAssertions;
using InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using Moq;
using Serilog;
using Shared.Extensions;
using Shared.Helpers;
using Xunit;
using Xunit.Priority;

namespace InputDataModel.Autodictor.Test
{
    //"viewRules": [
    //{
    //    "id": 1,
    //    "startPosition": 0,
    //    "count": 1,
    //    "batchSize": 1000,
    //    "requestOption": {
    //        "format": "cp866",
    //        "maxBodyLenght": 245,
    //        "header": "0x{AddressDevice:X2}0x{NbyteFull:X2} ",
    //        "body": "  0x03{Hour:D2}0x3A{Minute:D2}",
    //        "footer": "0x{CRCMod256:X2}"
    //    },
    //    "responseOption": {
    //        "format": "X2",
    //        "lenght": 16,
    //        "timeRespone": 2000,
    //        "body": "0246463038254130373741434B454103"
    //    }
    //}
    public class ViewRuleTest
    {
        //private static AdInputType _inTypeBase =
        //   new AdInputType
        //   {
        //       Id = 1,
        //       Lang = Lang.Ru,
        //       PathNumber = "5",
        //       NumberOfTrain = "245",
        //       ArrivalTime = DateTime.Parse("10:20"),
        //       DepartureTime = DateTime.Parse("11:52")
        //   };

        private readonly ILogger _logger;



        #region ctor

        public ViewRuleTest()
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
                        PathNumber = "5",
                        NumberOfTrain = "245",
                        ArrivalTime = DateTime.Parse("10:20"),
                        DepartureTime = DateTime.Parse("11:52")
                    }
                },
                new RequestOption
                {
                    Format = "cp866",
                    MaxBodyLenght = 245,
                    Header = "0x{AddressDevice:X2}0x{NbyteFull:X2} ",
                    Body = "  0x03{NumberOfTrain}0x3A{PathNumber}",
                    Footer = "0x{CRCMod256:X2}"
                },
                "HEX",
                "090C202020033234353A3582"
            },
            new object[]
            {
                new List<AdInputType>
                {
                    new AdInputType
                    {
                        Id = 1,
                        Lang = Lang.Ru,
                        PathNumber = "5",
                        NumberOfTrain = "245",
                        ArrivalTime = DateTime.Parse("10:20"),
                        DepartureTime = DateTime.Parse("11:52"),
                        Addition = new Addition {NameRu = "Дополнение"},
                    }
                },
                new RequestOption
                {
                    Format = "Windows-1251",
                    MaxBodyLenght = 245,
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%010C60EF03B0470000001E%110406NNNNN%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
                    Footer = "{CRCXorInverse:X2}\u0003"
                },
                "Windows-1251",
                "\u0002096E%010C60EF03B0470000001E%110406NNNNN%010000f001101f0024001E%100C05Дополнение  %0100002300000f0000001E%10030524583\u0003"
            },
            new object[]
            {
                new List<AdInputType>
                {
                    new AdInputType
                    {
                        Id = 1,
                        Lang = Lang.Ru,
                        PathNumber = "5",
                        NumberOfTrain = "245",
                        ArrivalTime = DateTime.Parse("10:20"),
                        DepartureTime = DateTime.Parse("11:52"),
                        Addition = new Addition {NameRu = "Дополнение"},
                        Note = new Note{NameRu = "Со всеми остановками кроме: Узуново, Ожерелье"}
                    }
                },
                new RequestOption
                {
                    Format = "Windows-1251",
                    MaxBodyLenght = 245,
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "010C60EF03B0470000001E%110406NNNNN{Note:[3^100][10^25]}%10{Addition:[0^GGG][5^FFF]}{TDepart:t}",  //[3^0x09][10^0x09]
                    Footer = "{CRCXorInverse:X2}\u0003"
                },
                "Windows-1251",
                "\u0002096C010C60EF03B0470000001E%110406NNNNNСо 100всем25и остановками кроме: Узуново, Ожерелье%10GGGДоFFFполнение11:529B\u0003"
            }

        };

        #endregion






        [Theory]
        [MemberData(nameof(GetDataRequestStringTestInputDatas))]
        public void GetDataRequestStringTest(
            List<AdInputType> inTypes,
            RequestOption requestOption,
            string formatExcpected,
            string requestStringExcpected)
        {
            // Arrange
            var address = "9";
            var option = new ViewRuleOption()
            {
                Id = 1,
                Count = 1,
                StartPosition = 0,
                BatchSize = 1000,
                RequestOption = requestOption,
                ResponseOption = null
            };
            var viewRule = new ViewRule(address, option, _logger);

            //Act
            var requests = viewRule.GetDataRequestString(inTypes).ToList();
            var firstRequest = requests.FirstOrDefault();
            var requestString = firstRequest?.StringRequest;
            var currentFormat = firstRequest?.RequestOption.GetCurrentFormat();


            //Asssert
            firstRequest.Should().NotBeNull();
            requestString.Should().NotBeNull();
            requestString.Should().Contain(requestStringExcpected);
            currentFormat.Should().Be(formatExcpected);
        }
    }
}