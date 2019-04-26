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
using Shared.CrcCalculate;
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
    public class EkrimViewRuleTest
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
            //new object[] 
            //{
            //   new List<AdInputType>
            //   {
            //        new AdInputType
            //        {
            //            Id = 1,
            //            Lang = Lang.Ru,
            //            NumberOfTrain = "245",
            //            StationArrival = new Station{NameRu = "КАЗАНЬ"},
            //            ArrivalTime = DateTime.Parse("20:50"),
            //        }
            //    },
            //    new RequestOption
            //    {
            //        Format = "Windows-1251",
            //        MaxBodyLenght = 245,
            //        Header = "0xFF0xFF0x020x1B0x57",
            //        Body = "{StationArrival}0x09{NumberOfTrain}0x09{TArrival:t}0x09",
            //        Footer = "0x03{CRCXor:X2}0x1F"  //[0x02-0x03]
            //    },
            //    "HEX",
            //    "090C202020033234353A3582"
            //},
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
                    Footer = "0x030x{CRCXorInverse[0x02-0x03]:X2}0x1F"
                },
                "HEX",
                "FFFF021B57CAC0C7C0CDDC09320932303A35300903561F"
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
            var resultBuffer = requestString.ConvertString2ByteArray(currentFormat);

            //Asssert
            firstRequest.Should().NotBeNull();
            requestString.Should().NotBeNull();
            requestString.Should().Contain(requestStringExcpected);
            currentFormat.Should().Be(formatExcpected);
        }
    }
}