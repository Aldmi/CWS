using System.Collections.Generic;
using System.Linq;
using ByRulesInseartedTest.Test.Datas;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class ManualValidatorTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
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
                        ValidatorName = "ManualEkrimValidator",
                        ManualEkrimValidator = new ManualEkrimValidatorOption()
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "041FFFFF1010FEFE051F",
                "HEX",
                "041FFFFF1010FEFE051F",   
                //RESPONSE
                new byte[]{0x01,0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12},
                0
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
            var viewRule = ViewRule<AdInputType>.Create(addressDevice, option, InTypeIndependentInsertsHandlerFactory, StringInsertModelExtDictionary, null, Logger);

            //Act
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArrayAsync().GetAwaiter().GetResult();
            var (isSuccess, isFailure, providerTransfer, error) = requestTransfers.FirstOrDefault();

            //Assert
            isSuccess.Should().BeTrue();


            var response = providerTransfer.Response;
            var responseInfo = response.Validator.Validate(expectedRespAray);
            responseInfo.IsOutDataValid.Should().BeTrue();

            var envelop = responseInfo.ToString();
        }



        [Fact]
        public void CheckError_Border_StartCh_0x02_NotFound()
        {
            //Arrange
            var option = new ViewRuleOption()
            {
                Id = 1,
                StartPosition = 0,
                Count = 1,
                BatchSize = 1,
                RequestOption = new RequestOption
                {
                    Header = "0xFF0xFF0x1B0x57",
                    Body = "{Addition}  {StationsCut}0x09{ExpectedTime:t}0x09{Note}0x09",
                    Footer = "0x030x{CRCXor:X2_BorderLeft<0x02-0x03>}0x1F",
                    Format = "Windows-1251",
                    MaxBodyLenght = 230
                },
                ResponseOption = new ResponseOption
                {
                    ValidatorName = "EqualValidator",
                    EqualValidator = new EqualResponseValidatorOption
                    {
                        Body = "061F",
                        Format = "HEX"
                    }
                }
            };
            var viewRule = ViewRule<AdInputType>.Create("5", option, InTypeIndependentInsertsHandlerFactory, StringInsertModelExtDictionary, null, Logger);

            //Act
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArrayAsync().GetAwaiter().GetResult();
            var (isSuccess, isFailure, providerTransfer, error) = requestTransfers.FirstOrDefault();

            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Not Found startCh= 0x02");
        }
    }
}