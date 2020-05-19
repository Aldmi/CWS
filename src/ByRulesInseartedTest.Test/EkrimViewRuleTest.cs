using System.Collections.Generic;
using System.Linq;
using ByRulesInseartedTest.Test.Datas;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class EkrimViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Казанский перон дальние Peron.D.P1 запрос 1
            new object[]
            {
                "5",
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
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "061F",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "041FFFFF1010FEFE051F",
                "HEX",
                "041FFFFF1010FEFE051F",   
                //RESPONSE
                new byte[]{0x06, 0x1F},
                0
            },
            // Казанский перон дальние Peron.D.P1 запрос 2
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0xFF0xFF0x020x1B0x57",
                        Body = "{StationsCut}0x09{NumberOfTrain}0x09{ExpectedTime:t}0x09",
                        Footer = "0x030x{CRCXor:X2_BorderExclude}0x1F",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "061F",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "FFFF021B57CFE8F2E5F02DCCEEF1EAE2E0093435360931353A343009039E1F",
                "HEX",
                "0xFF0xFF0x020x1B0x57Питер-Москва0x094560x0915:400x090x030x9E0x1F",   
                //RESPONSE
                new byte[]{0x06, 0x1F},
               3
            },
             //Казанский перон пригород Peron.P.P7 запрос 2
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0xFF0xFF0x020x1B0x57",
                        Body = "{Addition}  {StationsCut}0x09{ExpectedTime:t}0x09{Note}0x09",
                        Footer = "0x030x{CRCXor:X2_BorderExclude}0x1F",
                        Format = "Windows-1251",
                        MaxBodyLenght = 230
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "061F",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,
                //REQUEST
                "FFFF021B57202020CFE8F2E5F02DCCEEF1EAE2E00931353A343009D1F2E0EDF6E8FF20312CD1F2E0EDF6E8FF20322CD1F2E0EDF6E8FF20332CD1F2E0EDF6E8FF20342CD1F2E0EDF6E8FF20352CD1F2E0EDF6E8FF20362CD1F2E0EDF6E8FF20370903561F",
                "HEX",
                "0xFF0xFF0x020x1B0x57   Питер-Москва0x0915:400x09Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 70x090x030x560x1F",
                //RESPONSE
                new byte[]{0x06, 0x1F},
                4
            },
            //Казанский многострочка дальние Dalnie.Otpr1.6Str запрос 2
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0xFF0xFF0x020x1B0x57",
                        Body = "0x{MATH(rowNumber+64):X1}0x46{NumberOfTrain}0x09{TypeAlias}0x09{StationArrival}0x09{TDepart:t}0x09{PathNumber}0x09{DelayTime:t}0x09",
                        Footer = "0x030x{CRCXor:X2_BorderExclude}0x1F",
                        Format = "Windows-1251",
                        MaxBodyLenght = 230
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "061F",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "FFFF021B57414634353609D1CACED009CCEEF1EAE2E00931363A3138093509303A313509034D1F",
                "HEX",
                "0xFF0xFF0x020x1B0x570x410x464560x09СКОР0x09Москва0x0916:180x0950x090:150x090x030x4D0x1F",   
                //RESPONSE
                new byte[]{0x06, 0x1F},
                6
            }

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

            byte[] expectedRespAray,

            int expectedCountInseartedData)
        {
            //Arrange
            var viewRule = ViewRule<AdInputType>.Create(addressDevice, option, InTypeIndependentInsertsHandlerFactory, StringInsertModelExtDictionary, Logger);

            //Act
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArrayAsync().GetAwaiter().GetResult();
            var rt = requestTransfers.FirstOrDefault();
            var responseInfo = rt.Response.Validator.Validate(expectedRespAray);

            //Assert
            rt.Request.StrRepresentBase.Str.Should().Be(expectedRequestStrRepresentBase);
            rt.Request.StrRepresentBase.Format.Should().Be(option.RequestOption.Format);
            rt.Request.StrRepresent.Str.Should().Be(expectedRequestStrRepresent);
            rt.Request.StrRepresent.Format.Should().Be(expectedRequestStrRepresentFormat);
            rt.Request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(inputTypes.Count);
            foreach (var processedItem in rt.Request.ProcessedItemsInBatch.ProcessedItems)
            {
                processedItem.InseartedData.Where(pair => pair.Key != "MATH").ToList().Count.Should().Be(expectedCountInseartedData);
            }

            responseInfo.IsOutDataValid.Should().BeTrue();
        }
    }
}