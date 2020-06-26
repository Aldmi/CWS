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

    /// <summary>
    /// Тест табло Для Васильево
    /// по протоколу {NbyteFullLastCalc:X2}{CRCMod256:X2_hex_Border_StartOnly}{CRC8FullPoly:X2_hex_Border_StartOnly}0x01{(rowNumber+128):X2}0x5a0x120x030xa00x000x000x000x000x000x020x000x100x80{Platform}
    /// </summary>
    public class VasilievoTableTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            ////БЕЗ ВСТАВОК в ФОРМАТЕ HEX (очистка)
            //new object[]
            //{
            //    "5",
            //    new ViewRuleOption()
            //    {
            //        Id = 1,
            //        StartPosition = 0,
            //        Count = 1,
            //        BatchSize = 1,
            //        RequestOption = new RequestOption
            //        {
            //            Header = "{NbyteFullLastCalc:X2}{CRCMod256:X2_hex_Border_StartOnly}{CRC8FullPoly:X2_hex_Border_StartOnly}",
            //            Body = "01815a0800000000000001ff0020",
            //            Footer = "",
            //            Format = "HEX",
            //            MaxBodyLenght = 245
            //        },
            //        ResponseOption = new ResponseOption
            //        {
            //            ValidatorName = "EqualValidator",
            //            EqualValidator = new EqualResponseValidatorOption
            //            {
            //                Body = "01",
            //                Format ="HEX"
            //            }
            //        }
            //    },

            //    GetData4ViewRuleTest.InputTypesDefault,  
            //    //REQUEST
            //    "1104AF01815a0800000000000001ff0020", //11 04 af 01 81 5a 08 00 00 00 00 00 00 01 ff 00 20
            //    "HEX",
            //    "1104AF01815a0800000000000001ff0020", 
            //    //RESPONSE
            //    new byte[]{ 0x05, 0x04, 0x83, 0x8C },

            //    0
            //},

            ////БЕЗ ВСТАВОК в ФОРМАТЕ HEX (НОМЕР ПОЕЗДА)
            //new object[]
            //{
            //    "5",
            //    new ViewRuleOption()
            //    {
            //        Id = 1,
            //        StartPosition = 0,
            //        Count = 1,
            //        BatchSize = 1,
            //        RequestOption = new RequestOption
            //        {
            //            Header = "{NbyteFullLastCalc:X2}{CRCMod256:X2_hex_Border_StartOnly}{CRC8FullPoly:X2_hex_Border_StartOnly}",
            //            Body = "01815A12030000000000010100188031",
            //            Footer = "",
            //            Format = "HEX",
            //            MaxBodyLenght = 245
            //        },
            //        ResponseOption = new ResponseOption
            //        {
            //            ValidatorName = "EqualValidator",
            //            EqualValidator = new EqualResponseValidatorOption
            //            {
            //                Body = "{NbyteFullLastCalc:X2}{CRCMod256:X2_hex_Border_StartOnly2}{CRC8FullPoly:X2_hex_Border_StartOnly2}02014F4B00",
            //                Format ="HEX"
            //            }
            //        }
            //    },

            //    GetData4ViewRuleTest.InputTypesDefault,  
            //    //REQUEST
            //    "13BC9501815A12030000000000010100188031",
            //    "HEX",
            //    "13BC9501815A12030000000000010100188031",
            //    //RESPONSE
            //    new byte[]{ 0x08, 0x9d, 0x11, 0x02, 0x01, 0x4f, 0x4b, 0x00 },

            //    0
            //},

            //ВСТАВКА NumberOfTrain в ФОРМАТЕ Windows-1251
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
                        Header = "0x{NbyteFullLastCalc:X2}0x{CRC8FullPoly:X2_Border_StartOnly}0x{CRCMod256:X2_Border_StartOnly}",
                        Body = "0x010x810x5a0x080x000x000x000x000x000x000x01{NumberOfTrain}",
                        Footer = "",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "{NbyteFullLastCalc:X2}{CRCMod256:X2_hex_Border_StartOnly2}{CRC8FullPoly:X2_hex_Border_StartOnly2}02014F4B00",
                            Format ="HEX"
                        }
                    }
                },

                GetData4ViewRuleTest.InputTypesDefault,  
                //REQUEST
                "11A98401815A0800000000000001343536",
                "HEX",
                "0x110xA90x840x010x810x5a0x080x000x000x000x000x000x000x01456", 
                //RESPONSE
                new byte[]{ 0x08, 0x9d, 0x11, 0x02, 0x01, 0x4f, 0x4b, 0x00 },

                1
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