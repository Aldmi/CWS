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
    public class DispSysViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Хабаровск Perehod.P3.P5 запрос 1
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
                        Header = "\u0002{AddressDevice:X2}{Nchar:X2}",
                        Body = "%30%010C60EF03B0470000001E%110406NNNNN%01000023{MATH(rowNumber*24-23):X3}{MATH(rowNumber*24-13):X3}0020001E%10{NumberOfCharacters:X2}01\\\"{NumberOfTrain}\\\"%01024030{MATH(rowNumber*24-23):X3}{MATH(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\" \\\"%01062086{MATH(rowNumber*24-23):X3}{MATH(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TArrival:t}\\\"%010870AB{MATH(rowNumber*24-23):X3}{MATH(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TDepart:t}\\\"",
                        Footer = "{CRCXorInverse:X2}\u0003",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "{AddressDevice:X2}0246463038254130373741434B454103",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,
                //REQUEST
                "\u000205AC%30%010C60EF03B0470000001E%110406NNNNN%0100002300100B0020001E%100301456%0102403000100B0000001E%100101 %0106208600100B0000001E%10050115:25%010870AB00100B0000001E%10050116:18FB\u0003",
                "Windows-1251",
                "\u000205AC%30%010C60EF03B0470000001E%110406NNNNN%0100002300100B0020001E%100301456%0102403000100B0000001E%100101 %0106208600100B0000001E%10050115:25%010870AB00100B0000001E%10050116:18FB\u0003",   
                //RESPONSE
                new byte[]{ 0x05, 0x02, 0x46, 0x46, 0x30, 0x38, 0x25, 0x41, 0x30, 0x37, 0x37, 0x41, 0x43, 0x4B, 0x45, 0x41, 0x03 },

                3
            },
            //Хабаровск Perehod.P3.P5 запрос 2
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
                        Header = "\u0002{AddressDevice:X2}{Nchar:X2}",
                        Body = "%010000F0{MATH(rowNumber*24-11):X3}{MATH(rowNumber*24-1):X3}0024001E%10{NumberOfCharacters:X2}01\\\"{Addition} {StationDeparture}-{StationArrival}\\\"%010AC0D5{MATH(rowNumber*24-23):X3}{MATH(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TypeAlias}\\\"%010D60F0{MATH(rowNumber*24-23):X3}{MATH(rowNumber*24-13):X3}0020001E%10{NumberOfCharacters:X2}01\\\"{DelayTime:Min}\\\"",
                        Footer = "{CRCXorInverse:X2}\u0003",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "0246463038254130373741434B454103",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,
                //REQUEST
                "\u0002056D%010000F000D0170024001E%100D01 Питер-Москва%010AC0D500100B0000001E%100401СКОР%010D60F000100B0020001E%1002011309\u0003",
                "Windows-1251",
                "\u0002056D%010000F000D0170024001E%100D01 Питер-Москва%010AC0D500100B0000001E%100401СКОР%010D60F000100B0020001E%1002011309\u0003",
                //RESPONSE
                new byte[]{0x02, 0x46, 0x46, 0x30, 0x38, 0x25, 0x41, 0x30, 0x37, 0x37, 0x41, 0x43, 0x4B, 0x45, 0x41, 0x03 },

                5
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
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
            var rt = requestTransfers.FirstOrDefault();
            var responseInfo = rt.Response.Validator.Validate(expectedRespAray);

            //Assert
            rt.Request.StrRepresent.Str.Should().Be(expectedRequestStrRepresent);
            rt.Request.StrRepresent.Format.Should().Be(expectedRequestStrRepresentFormat);
            rt.Request.StrRepresentBase.Str.Should().Be(expectedRequestStrRepresentBase);
            rt.Request.StrRepresentBase.Format.Should().Be(option.RequestOption.Format);
            rt.Request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(inputTypes.Count);
            foreach (var processedItem in rt.Request.ProcessedItemsInBatch.ProcessedItems)
            {
                processedItem.InseartedData.Where(pair => pair.Key != "MATH").ToList().Count.Should().Be(expectedCountInseartedData);
            }

            responseInfo.IsOutDataValid.Should().BeTrue();
        }

    }
}