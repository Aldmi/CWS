﻿using System.Collections.Generic;
using System.Linq;
using ByRulesInseartedTest.Test.Datas;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class SinergoViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData //Царицыно низкоуровнеквый протокол самих табло
        public static IEnumerable<object[]> Datas => new[]
        {
            new object[]
            {
                "4",                                          
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = ":{AddressDevice:X2}rs2=5,",
                        Body ="Test",
                        Footer = "*{CRC8Bit:X2_BorderIncl_Sinergo}0x0D",
                        Format = "ascii",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = ":{AddressDevice:X2}ok*{CRC8Bit:X2_BorderIncl_Sinergo}0x0D",
                            Format ="ascii"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "3A30347273323D352C546573742A31440D",
                "HEX",
                ":04rs2=5,Test*1D0x0D",   

                //RESPONSE
                new byte[]{0x3A, 0x30, 0x34, 0x6F, 0x6B, 0x2A, 0x41, 0x32, 0x0D},
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
            var viewRule = ViewRule<AdInputType>.Create(addressDevice, option, InTypeIndependentInsertsHandlerFactory, StringInsertModelExtDictionary, Logger);

            //Act
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
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