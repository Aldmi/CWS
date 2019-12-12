using System;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Autodictor.IndependentInseartsHandlers;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Moq;
using Serilog;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class SinergoViewRuleTest
    {
        private readonly ILogger _logger;
        private IIndependentInsertsHandler _inTypeIndependentInsertsHandler;


        #region ctor
        public SinergoViewRuleTest()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(loger => loger.Debug(""));
            mock.Setup(loger => loger.Error(""));
            mock.Setup(loger => loger.Warning(""));
            _logger = mock.Object;

            _inTypeIndependentInsertsHandler = new AdInputTypeIndependentInsertsHandler();
        }
        #endregion



        [Fact]
        public void CreateStringRequestEmptyRequestOptionTest()
        {
            //Arrange
            string addressDevice = "4";
            var viewRuleOption = new ViewRuleOption()
            {
                Id = 1,
                StartPosition = 0,
                Count = 1,
                BatchSize = 1,
                RequestOption = new RequestOption
                {
                    Header = ":{AddressDevice:X2}rs2=5,",
                    Body ="Test",
                    Footer = "*{CRC8Bit[:-*]:X2}0x0D",
                    Format = "ascii",
                    MaxBodyLenght = 245
                },
                ResponseOption = new ResponseOption
                {
                    Format = "ascii",
                    Lenght = 9,
                    Body = ":{AddressDevice:X2}ok*{CRC8Bit[:-*]:X2}0x0D"
                }
            };

            var viewRule = new ViewRule<AdInputType>(addressDevice, viewRuleOption, _inTypeIndependentInsertsHandler, _logger);

            //Act
            var requestTransfer = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
            var firstItem = requestTransfer.FirstOrDefault();

            //Assert
            requestTransfer.Should().NotBeNull();
            requestTransfer.Length.Should().Be(1);
            firstItem.Request.StrRepresent.Str.Should().Be("3A30347273323D352C546573742A31440D");
            firstItem.Request.StrRepresent.Format.Should().Be("HEX");
            firstItem.Request.StrRepresentBase.Str.Should().Be(":04rs2=5,Test*1D0x0D");
            firstItem.Request.StrRepresentBase.Format.Should().Be(viewRuleOption.RequestOption.Format);
            firstItem.Request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(1);

            firstItem.Response.StrRepresent.Str.Should().Be("3A30346F6B2A41320D");
            firstItem.Response.StrRepresent.Format.Should().Be("HEX");
        }



        //"requestOption": {
        //    "format": "Windows-1251",
        //    "maxBodyLenght": 245,
        //    "header": "0xFF0xFF0x020x1B0x57",
        //    "body": "{StationsCut}0x09{NumberOfTrain}0x09",
        //    "footer": "0x030x{CRCXor(0x02-0x03):X2}0x1F"
        //},
        //"responseOption": {
        //    "format": "HEX",
        //    "lenght": 2,
        //    "timeRespone": 1000,
        //    "body": "061F"

        
        [Fact]
        public void EkrimTest()
        {
            //Arrange
            string addressDevice = "4";
            var viewRuleOption = new ViewRuleOption()
            {
                Id = 1,
                StartPosition = 0,
                Count = 1,
                BatchSize = 1,
                RequestOption = new RequestOption
                {
                    Header = "0xFF0xFF0x020x1B0x57",
                    Body ="{StationsCut}0x09{NumberOfTrain}0x09",
                    Footer = "0x030x{CRCXor(0x02-0x03):X2}0x1F",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                ResponseOption = new ResponseOption
                {
                    Format = "HEX",
                    Lenght = 2,
                    Body = "061F"
                }
            };

            var viewRule = new ViewRule<AdInputType>(addressDevice, viewRuleOption, _inTypeIndependentInsertsHandler, _logger);

            //Act
            var requestTransfer = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
            var firstItem = requestTransfer.FirstOrDefault();

            //Assert
            requestTransfer.Should().NotBeNull();
            requestTransfer.Length.Should().Be(1);
            firstItem.Request.StrRepresent.Str.Should().Be("");
            firstItem.Request.StrRepresent.Format.Should().Be("");
            firstItem.Request.StrRepresentBase.Str.Should().Be("");
            firstItem.Request.StrRepresentBase.Format.Should().Be(viewRuleOption.RequestOption.Format);
            firstItem.Request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(1);

            firstItem.Response.StrRepresent.Str.Should().Be("061F");
            firstItem.Response.StrRepresent.Format.Should().Be("HEX");
        }

    }
}