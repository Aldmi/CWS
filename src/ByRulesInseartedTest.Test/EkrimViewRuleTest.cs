using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Factory;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Moq;
using Serilog;
using Shared.Services.StringInseartService;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class EkrimViewRuleTest :BaseViewRuleTest
    {

        //string addressDevice,
        //ViewRuleOption option,
        //List<AdInputType> inputTypes,
        //string expectedRequestStrRepresent,
        //string expectedRequestStrRepresentFormat,
        //string expectedRequestStrRepresentBase,

        //string expectedRespStrRepresent,
        //string expectedRespStrRepresentFormat,

        //int expectedCountInseartedData)


        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Казанский перон дальние Peron.D.P1 запрос 1
            new object[]
            {
                "5",                                           //addressDevice
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "",
                        Body = "041FFFFF1010FEFE051F",
                        Footer = "",
                        Format = "HEX",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        Format = "HEX",
                        Lenght = 2,
                        Body = "061F"
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "041FFFFF1010FEFE051F",  
                "HEX",                                                                                                                                                     
                "041FFFFF1010FEFE051F",   
                //RESPONSE
                "061F",
                "HEX",
                0                                                                                                                                                                  
            },
            //// Казанский перон дальние Peron.D.P1 запрос 2
            //new object[]
            //{
            //    "5",                                           //addressDevice
            //    new RequestOption                              //requestOption
            //    {
            //        Header = "0xFF0xFF0x020x1B0x57",
            //        Body = "{StationsCut}0x09{NumberOfTrain}0x09{ExpectedTime:t}0x09",
            //        Footer = "0x030x{CRCXor[0x02-0x03]:X2}0x1F",
            //        Format = "Windows-1251",
            //        MaxBodyLenght = 245
            //    },
            //    GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
            //    "FFFF021B57CCEEF1EAE2E02DCFE8F2E5F0093435360931363A31380903901F",   //expectedStrRepresent
            //    "HEX",                                                                                                                                                      //expectedStrRepresentFormat
            //    "0xFF0xFF0x020x1B0x57Москва-Питер0x094560x0916:180x090x030x900x1F",   //expectedStrRepresentBase
            //    3                                                                                                                                                                  //expectedCountInseartedData
            //},
            //// Казанский перон пригород Peron.P.P7 запрос 1
            //new object[]
            //{
            //    "5",                                           //addressDevice
            //    new RequestOption                              //requestOption
            //    {
            //        Header = "",
            //        Body = "041FFFFF1010F8F8051F",
            //        Footer = "",
            //        Format = "HEX",
            //        MaxBodyLenght = 245
            //    },
            //    GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
            //    "041FFFFF1010F8F8051F",   //expectedStrRepresent
            //    "HEX",                                                                                                                                                      //expectedStrRepresentFormat
            //    "041FFFFF1010F8F8051F",   //expectedStrRepresentBase
            //    0                                                                                                                                                                   //expectedCountInseartedData
            //},
            //// Казанский перон пригород Peron.P.P7 запрос 2
            //new object[]
            //{
            //    "5",                                           //addressDevice
            //    new RequestOption                              //requestOption
            //    {
            //        Header = "0xFF0xFF0x020x1B0x57",
            //        Body = "{Addition} {StationsCut}0x09{ExpectedTime:t}0x09{Note}0x09",
            //        Footer = "0x030x{CRCXor[0x02-0x03]:X2}0x1F",
            //        Format = "Windows-1251",
            //        MaxBodyLenght = 230
            //    },
            //    GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
            //    "FFFF021B572020CCEEF1EAE2E02DCFE8F2E5F00931363A313809D1F2E0EDF6E8FF20312CD1F2E0EDF6E8FF20322CD1F2E0EDF6E8FF20332CD1F2E0EDF6E8FF20342CD1F2E0EDF6E8FF20352CD1F2E0EDF6E8FF20362CD1F2E0EDF6E8FF20370903781F",   //expectedStrRepresent
            //    "HEX",                                                                                                                                                      //expectedStrRepresentFormat
            //    "0xFF0xFF0x020x1B0x57  Москва-Питер0x0916:180x09Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 70x090x030x780x1F",   //expectedStrRepresentBase
            //    4                                                                                                                                                                  //expectedCountInseartedData
            //},
            ////Казанский многострочка дальние Dalnie.Otpr1.6Str запрос 2
            //new object[]
            //{
            //    "5",                                           //addressDevice
            //    new RequestOption                              //requestOption
            //    {
            //        Header = "0xFF0xFF0x020x1B0x57",
            //        Body = "0x{(rowNumber+64):X1}0x46{NumberOfTrain}0x09{TypeAlias}0x09{StationArrival}0x09{TDepart:t}0x09{PathNumber}0x09{DelayTime:t}0x09",
            //        Footer = "0x030x{CRCXor[0x02-0x03]:X2}0x1F",
            //        Format = "Windows-1251",
            //        MaxBodyLenght = 230
            //    },
            //    GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
            //    "FFFF021B574146343536092009CCEEF1EAE2E00931363A3138092009200903531F",   //expectedStrRepresent
            //    "HEX",                                                                                                                                                      //expectedStrRepresentFormat
            //    "0xFF0xFF0x020x1B0x570x410x464560x09 0x09Москва0x0916:180x09 0x09 0x090x030x530x1F",   //expectedStrRepresentBase
            //    7                                                                                                                                                                 //expectedCountInseartedData
            //},

        };
        #endregion



        [Theory]
        [MemberData(nameof(Datas))]
        public void CreateStringRequestTest(
            string addressDevice,
            ViewRuleOption option,
            List<AdInputType> inputTypes,
            string expectedRequestStrRepresent,
            string expectedRequestStrRepresentFormat,
            string expectedRequestStrRepresentBase,

            string expectedRespStrRepresent,
            string expectedRespStrRepresentFormat,

            int expectedCountInseartedData)
        {
            //Arrange
            var viewRule = ViewRule<AdInputType>.Create(addressDevice, option, InTypeIndependentInsertsHandlerFactory, Logger);

            //Act
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
            var rt = requestTransfers.FirstOrDefault();

            //Assert
            rt.Request.StrRepresent.Str.Should().Be(expectedRequestStrRepresent);
            rt.Request.StrRepresent.Format.Should().Be(expectedRequestStrRepresentFormat);
            rt.Request.StrRepresentBase.Str.Should().Be(expectedRequestStrRepresentBase);
            rt.Request.StrRepresentBase.Format.Should().Be(option.RequestOption.Format);
            rt.Request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(inputTypes.Count);
            foreach (var processedItem in rt.Request.ProcessedItemsInBatch.ProcessedItems)
            {
                processedItem.InseartedData.Count.Should().Be(expectedCountInseartedData);
            }

            rt.Response.StrRepresent.Str.Should().Be(expectedRespStrRepresent);
            rt.Response.StrRepresent.Format.Should().Be(expectedRespStrRepresentFormat);
        }

    }
}