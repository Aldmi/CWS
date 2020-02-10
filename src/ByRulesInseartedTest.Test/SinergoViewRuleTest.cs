using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ByRulesInseartedTest.Test.Datas;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Factory;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using FluentAssertions;
using Moq;
using Serilog;
using Shared.Services.StringInseartService;
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
                },

                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "3A30347273323D352C546573742A31440D",
                "HEX",
                ":04rs2=5,Test*1D0x0D",   
                //RESPONSE
                "3A30346F6B2A41320D",
                "HEX",
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
                processedItem.InseartedData.Where(pair => pair.Key != "MATH").ToList().Count.Should().Be(expectedCountInseartedData);
            }

            rt.Response.StrRepresent.Str.Should().Be(expectedRespStrRepresent);
            rt.Response.StrRepresent.Format.Should().Be(expectedRespStrRepresentFormat);
        }

    }
}