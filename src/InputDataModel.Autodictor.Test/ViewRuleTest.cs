//using System;
//using System.Collections.Generic;
//using System.Linq;
//using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
//using FluentAssertions;
//using InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules;
//using InputDataModel.Autodictor.Entities;
//using InputDataModel.Autodictor.Model;
//using Moq;
//using Serilog;
//using Xunit;

//namespace InputDataModel.Autodictor.Test
//{
//    public class ViewRuleTest
//    {
//        private readonly ILogger _logger;



//        #region ctor

//        public ViewRuleTest()
//        {
//            var mock = new Mock<ILogger>();
//            mock.Setup(loger => loger.Debug(""));
//            mock.Setup(loger => loger.Error(""));
//            mock.Setup(loger => loger.Warning(""));
//            _logger = mock.Object;

//        }

//        #endregion



//        #region SharedData4Tests

//        //List<AdInputType> inTypes, RequestOption requestOption, string formatExcpected, string requestStringExcpected
//        public static IEnumerable<object[]> GetDataRequestStringTestInputDatas => new[]
//        {
//            //ИнформСвязь
//            new object[]
//            {
//               new List<AdInputType>
//               {
//                    new AdInputType
//                    {
//                        Id = 1,
//                        Lang = Lang.Ru,
//                        PathNumber = "5",
//                        NumberOfTrain = "245",
//                        ArrivalTime = DateTime.Parse("10:20"),
//                        DepartureTime = DateTime.Parse("11:52")
//                    }
//                },
//                new RequestOption
//                {
//                    Format = "cp866",
//                    MaxBodyLenght = 245,
//                    Header = "0x{AddressDevice:X2}0x{NbyteFull:X2} ",
//                    Body = "  0x03{NumberOfTrain}0x3A{PathNumber}",
//                    Footer = "0x{CRCMod256:X2}"
//                },
//                new ResponseOption
//                {
//                    Format = "HEX",
//                    Lenght = 4,
//                    Body = "0x{AddressDevice:X2}0x040x830x{CRCMod256:X2}",
//                },
//                "HEX",
//                "090C202020033234353A3582",
//                "HEX",
//                "09048390"
//            },
//            //Дисплейные системы
//            new object[]
//            {
//                new List<AdInputType>
//                {
//                    new AdInputType
//                    {
//                        Id = 1,
//                        Lang = Lang.Ru,
//                        PathNumber = "5",
//                        NumberOfTrain = "245",
//                        ArrivalTime = DateTime.Parse("10:20"),
//                        DepartureTime = DateTime.Parse("11:52"),
//                        Addition = new Addition {NameRu = "Дополнение"},
//                    }
//                },
//                new RequestOption
//                {
//                    Format = "Windows-1251",
//                    MaxBodyLenght = 245,
//                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
//                    Body = "%010C60EF03B0470000001E%110406NNNNN%010000f001101f0024001E%10{NumberOfCharacters:X2}05\\\"{Addition} {Stations}\\\"%0100002300000f0000001E%10{NumberOfCharacters:X2}05\\\"{NumberOfTrain}\\\"",
//                    Footer = "{CRCXorInverse:X2}\u0003"
//                },
//                new ResponseOption
//                {
//                    Format = "HEX",
//                    Lenght = 16,
//                    Body = "0246463038254130373741434B454103",
//                },
//                "Windows-1251",
//                "\u0002096E%010C60EF03B0470000001E%110406NNNNN%010000f001101f0024001E%100C05Дополнение  %0100002300000f0000001E%10030524583\u0003",
//                "HEX",
//                "0246463038254130373741434B454103"
//            },
//            //Тест вставки в строковые переменные ArrayCharInseart [3^100][10^25]
//            new object[]
//            {
//                new List<AdInputType>
//                {
//                    new AdInputType
//                    {
//                        Id = 1,
//                        Lang = Lang.Ru,
//                        PathNumber = "5",
//                        NumberOfTrain = "245",
//                        ArrivalTime = DateTime.Parse("10:20"),
//                        DepartureTime = DateTime.Parse("11:52"),
//                        Addition = new Addition {NameRu = "Дополнение"},
//                        Note = new Note{NameRu = "Со всеми остановками кроме: Узуново, Ожерелье"}
//                    }
//                },
//                new RequestOption
//                {
//                    Format = "Windows-1251",
//                    MaxBodyLenght = 245,
//                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
//                    Body = "010C60EF03B0470000001E%110406NNNNN{Note: ArrayCharInseart [3^100][10^25]}%10{Addition: ArrayCharInseart [0^GGG][5^FFF]}{TDepart:t}",  //[3^0x09][10^0x09]
//                    Footer = "{CRCXorInverse:X2}\u0003"
//                },
//                new ResponseOption
//                {
//                    Format = "HEX",
//                    Lenght = 16,
//                    Body = "0246463038254130373741434B454103",
//                },
//                "Windows-1251",
//                "\u0002096C010C60EF03B0470000001E%110406NNNNNСо 100всем25и остановками кроме: Узуново, Ожерелье%10GGGДоFFFполнение11:529B\u0003",
//                "HEX",
//                "0246463038254130373741434B454103"
//            },
//            //Тест вставки в строковые переменные EndLineCharInseart [3^100][10^25]
//            new object[]
//            {
//                new List<AdInputType>
//                {
//                    new AdInputType
//                    {
//                        Id = 1,
//                        Lang = Lang.Ru,
//                        PathNumber = "5",
//                        NumberOfTrain = "245",
//                        ArrivalTime = DateTime.Parse("10:20"),
//                        DepartureTime = DateTime.Parse("11:52"),
//                        Addition = new Addition {NameRu = "Дополнение"},
//                        Note = new Note{NameRu = "Со всеми остановками кроме: Узуново, Ожерелье"}
//                    }
//                },
//                new RequestOption
//                {
//                    Format = "Windows-1251",
//                    MaxBodyLenght = 245,
//                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
//                    Body = "010C60EF03B0470000001E%110406NNNNN{Note: EndLineCharInseart [15^HHH]}%10{Addition}{TDepart:t}",
//                    Footer = "{CRCXorInverse:X2}\u0003"
//                },
//                new ResponseOption
//                {
//                    Format = "HEX",
//                    Lenght = 16,
//                    Body = "0246463038254130373741434B454103",
//                },
//                "Windows-1251",
//                "\u0002096A010C60EF03B0470000001E%110406NNNNNСо всемиHHHостановкамиHHHкроме:HHHУзуново,HHHОжерелье %10Дополнение11:528E\u0003",
//                "HEX",
//                "0246463038254130373741434B454103"
//            }
//        };


//        //RequestOption requestOption, string formatExcpected, string requestStringExcpected
//        public static IEnumerable<object[]> GetCommandRequestStringTestInputDatas => new[]
//        {
//            new object[]
//            {
//                new RequestOption
//                {
//                    Format = "Windows-1251",
//                    MaxBodyLenght = 0,
//                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
//                    Body = "%23",
//                    Footer = "{CRCXorInverse:X2}\u0003"
//                },
//                new ResponseOption
//                {
//                    Format = "HEX",
//                    Lenght = 16,
//                    Body = "0246463038254130373741434B454103",
//                },
//                "Windows-1251",
//                "\u00020903%23D1\u0003",
//                "HEX",
//                "0246463038254130373741434B454103"
//            }
//        };

//        #endregion






//        [Theory]
//        [MemberData(nameof(GetDataRequestStringTestInputDatas))]
//        public void GetDataRequestStringTest(
//            List<AdInputType> inTypes,
//            RequestOption requestOption,
//            ResponseOption responseOption,
//            string requestFormatExcpected,
//            string requestStringExcpected,
//            string responseFormatExcpected,
//            string responseStringExcpected)
//        {
//            // Arrange
//            var address = "9";
//            var option = new ViewRuleOption()
//            {
//                Id = 1,
//                Count = 1,
//                StartPosition = 0,
//                BatchSize = 1000,
//                RequestOption = requestOption,
//                ResponseOption = responseOption
//            };
//            var viewRule = new ViewRule(address, option, _logger);


//            //Act
//            var transfers = viewRule.CreateProviderTransfer4Data(inTypes).ToList();
//            var firstTransfer = transfers.FirstOrDefault();

//            var requestString = firstTransfer?.Request.StrRepresent.Str;
//            var requestFormat = firstTransfer?.Request.StrRepresent.Format;

//            var responseString = firstTransfer?.Response.StrRepresent.Str;
//            var responseFormat = firstTransfer?.Response.StrRepresent.Format;
//            var responseLenght = (responseFormat == "HEX") ? (firstTransfer?.Response.Option.Lenght * 2) : firstTransfer?.Response.Option.Lenght;


//            //Asssert
//            firstTransfer.Should().NotBeNull();

//            requestString.Should().NotBeNull();
//            requestString.Should().Contain(requestStringExcpected);
//            requestFormat.Should().Be(requestFormatExcpected);

//            responseString.Should().NotBeNull();
//            responseString.Should().Contain(responseStringExcpected);
//            responseFormat.Should().Be(responseFormatExcpected);
//            responseString.Length.Should().Be(responseLenght);
//        }




//        [Theory]
//        [MemberData(nameof(GetCommandRequestStringTestInputDatas))]
//        public void GetCommandRequestStringTest(
//            RequestOption requestOption,
//            ResponseOption responseOption,
//            string requestFormatExcpected,
//            string requestStringExcpected,
//            string responseFormatExcpected,
//            string responseStringExcpected)
//        {
//            // Arrange
//            var address = "9";
//            var option = new ViewRuleOption()
//            {
//                Id = 1,
//                Count = 1,
//                StartPosition = 0,
//                BatchSize = 1000,
//                RequestOption = requestOption,
//                ResponseOption = responseOption
//            };
//            var viewRule = new ViewRule(address, option, _logger);


//            var transfer = viewRule.GetCommandRequestString();

//            var requestString = transfer?.Request.StrRepresent.Str;
//            var requestFormat = transfer?.Request.StrRepresent.Format;

//            var responseString = transfer?.Response.StrRepresent.Str;
//            var responseFormat = transfer?.Response.StrRepresent.Format;
//            var responseLenght = (responseFormat == "HEX") ? (transfer.Response.Option.Lenght * 2) : transfer.Response.Option.Lenght;


//            //Asssert
//            transfer.Should().NotBeNull();

//            requestString.Should().NotBeNull();
//            requestString.Should().Contain(requestStringExcpected);
//            requestFormat.Should().Be(requestFormatExcpected);

//            responseString.Should().NotBeNull();
//            responseString.Should().Contain(responseStringExcpected);
//            responseFormat.Should().Be(responseFormatExcpected);
//            responseString.Length.Should().Be(responseLenght);

//        }

//    }
//}