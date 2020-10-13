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
    public class InformSvayzViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Павелецкий Peron.P1 Data2
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
                        Header = "0x{AddressDevice:X2}0x{Nbyte:X2_BorderRightBeforeCrc_Math}",
                        Body = "0x03^{NumberOfTrain}^ {StationsCut}^{TArrival:t}^{TDepart:t}",
                        Footer = "0x{CRCMod256:X2_BorderLeft}",
                        Format = "cp866",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "{AddressDevice:X2}0483{CRCMod256:X2_BorderLeft}",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,  
                //REQUEST
                "0522035E3435365E208FA8E2A5E02D8CAEE1AAA2A05E31353A32355E31363A313844",
                "HEX",
                "0x050x220x03^456^ Питер-Москва^15:25^16:180x44", 
                //RESPONSE
                new byte[]{ 0x05, 0x04, 0x83, 0x8C },

                4                                                                                                                                                           
            },
            // Павелецкий Peron.P1 Data3
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
                        Header = "0x{AddressDevice:X2}0x{Nbyte:X2_BorderRightBeforeCrc_Math}",
                        Body = "0x03{Note}",
                        Footer = "0x{CRCMod256:X2_BorderLeft}",
                        Format = "cp866",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "{AddressDevice:X2}0483{CRCMod256:X2_BorderLeft}",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,    
                //REQUEST
                "05490391E2A0ADE6A8EF20312C91E2A0ADE6A8EF20322C91E2A0ADE6A8EF20332C91E2A0ADE6A8EF20342C91E2A0ADE6A8EF20352C91E2A0ADE6A8EF20362C91E2A0ADE6A8EF203750",
                "HEX",
                "0x050x490x03Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 70x50",                                                                   
                //RESPONSE
                new byte[]{ 0x05, 0x04, 0x83, 0x8C },

                1
            },
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

            var request = providerTransfer.Request;
            request.StrRepresent.Str.Should().Be(expectedRequestStrRepresent);
            request.StrRepresent.Format.Should().Be(expectedRequestStrRepresentFormat);
            request.StrRepresentBase.Str.Should().Be(expectedRequestStrRepresentBase);
            request.StrRepresentBase.Format.Should().Be(option.RequestOption.Format);
            request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(inputTypes.Count);
            foreach (var processedItem in request.ProcessedItemsInBatch.ProcessedItems)
            {
                processedItem.InseartedData.Where(pair => pair.Key != "MATH").ToList().Count.Should().Be(expectedCountInseartedData);
            }

            var response = providerTransfer.Response;
            var responseInfo = response.Validator.Validate(expectedRespAray);
            responseInfo.IsOutDataValid.Should().BeTrue();
        }
    }
}