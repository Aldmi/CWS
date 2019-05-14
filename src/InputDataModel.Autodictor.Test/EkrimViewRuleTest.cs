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
    //        "format": "Windows-1251",
    //        "maxBodyLenght": 245,
    //        "header": "0xFF0xFF0x020x1B0x57",
    //        "body": "{StationArrival}0x09{NumberOfTrain}0x09{TArrival:t}0x09",
    //        "footer": "0x030x{CRCXor[0x02-0x03]:X2}0x1F
    //    "responseOption": {
    //        "format": "HEX",
    //        "lenght": 2,
    //        "timeRespone": 2000,
    //        "body": "061F"
    //    }
    //}
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