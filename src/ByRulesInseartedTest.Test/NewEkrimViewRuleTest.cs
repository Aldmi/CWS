using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ByRulesInseartedTest.Test.Datas;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class NewEkrimViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Казанский перон дальние Peron.D.P1 запрос 1
            new object[]
            {
                "0",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0xFF0xFF0xFF0x0200{AddressDevice:X2}WEB",
                        Body = "Светлогорск-20x0914:010x09260x09Поезд проследует со всеми остановками.",
                        Footer = "0x03{CRCCcitt((0x02-0x03) Hex):X4}0x04",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "0xFF0xFF0xFF0x0208{AddressDevice:X2}0x060x03{CRCCcitt((0x02-0x03) Hex):X4}0x04",
                            Format = "Windows-1251"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "FFFFFF0230303030574542D1E2E5F2EBEEE3EEF0F1EA2D320931343A303109323609CFEEE5E7E420EFF0EEF1EBE5E4F3E5F220F1EE20E2F1E5ECE820EEF1F2E0EDEEE2EAE0ECE82E03B05B04",
                "HEX",
                "0xFF0xFF0xFF0x020000WEBСветлогорск-20x0914:010x09260x09Поезд проследует со всеми остановками.0x030xB00x5B0x04",   
                //RESPONSE
                 new byte[] {0xFF, 0xFF, 0xFF, 0x02, 0x30, 0x38, 0x30, 0x30, 0x06, 0x03, 0x14, 0x3E, 0x04},
                 "Valid: True  Type: EqualResponseInfo  Info= [FFFFFF02303830300603143E04]:HEX <---> [FFFFFF02303830300603143E04]:HEX",
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

            byte[] expectedRespArr,
            string expectedRespInfoString,
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
                processedItem.InseartedData.Where(pair => pair.Key != "MATH").ToList().Count.Should().Be(expectedCountInseartedData);
            }

            var respInfo= rt.Response.Validator.Validate(expectedRespArr);
            respInfo.IsOutDataValid.Should().BeTrue();
            respInfo.ToString().Should().Be(expectedRespInfoString);

        }
    }
}