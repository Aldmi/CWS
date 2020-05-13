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
    public class NewEkrimViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Калининград. Перонное табло
            new object[]
            {
                "2",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0xFF0xFF0xFF0x0200{AddressDevice:X2}WEB",
                        Body = "10x0914:010x09260x093",
                        Footer = "0x03{CRCCcitt:X4_Border}0x04",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "0xFF0xFF0xFF0x0208{AddressDevice:X2}0x060x03{CRCCcitt:X4_Border}0x04",
                            Format = "Windows-1251"
                        }
                    }
                },
                GetData4ViewRuleTest.InputTypesDefault,
                //REQUEST
                "FFFFFF0230303032574542310931343A30310932360933033445384204",
                "HEX",
                "0xFF0xFF0xFF0x020002WEB10x0914:010x09260x0930x034E8B0x04",
               // RESPONSE
                new byte[] {0xFF, 0xFF, 0xFF, 0x02, 0x30, 0x38, 0x30, 0x32, 0x06, 0x03, 0x37, 0x32, 0x35, 0x43, 0x04 },
                0
            },

            // Питер. табло ИТ_1_3_4 (Обухово)
            new object[]
            {
                "3",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0xFF0xFF0xFF0x0200{AddressDevice:X2}WCB",
                        Body = "Любань0x0912:220x09Поезд следует со всеми остановками",
                        Footer = "0x03{CRCCcitt:X4_Border}0x04",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "0xFF0xFF0xFF0x0208{AddressDevice:X2}0x060x03{CRCCcitt:X4_Border}0x04",
                            Format = "Windows-1251"
                        }
                    }
                },
                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "FFFFFF0230303033574342CBFEE1E0EDFC0931323A323209CFEEE5E7E420F1EBE5E4F3E5F220F1EE20E2F1E5ECE820EEF1F2E0EDEEE2EAE0ECE8033335394304",
                "HEX",
                "0xFF0xFF0xFF0x020003WCBЛюбань0x0912:220x09Поезд следует со всеми остановками0x03359C0x04",   
                //RESPONSE
                new byte[] {0xFF, 0xFF, 0xFF, 0x02, 0x30, 0x38, 0x30, 0x33, 0x06, 0x03, 0x34, 0x31, 0x36, 0x44, 0x04},

                0
            },

            // Питер. табло LD45 (Обухово). УСТАНОВКА СОЕДИНЕНИЯ
            new object[]
            {
                "3",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    RequestOption = new RequestOption
                    {
                        Header = "0x100x10{AddressDevice:X2}{AddressDevice:X2}0x050x1F",
                        Body = "",
                        Footer = "",
                        Format = "Windows-1251",
                        MaxBodyLenght = 245
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "0x060x1F",
                            Format = "Windows-1251"
                        }
                    }
                },
                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "101030333033051F",
                "HEX",
                "0x100x1003030x050x1F",   
                //RESPONSE
                new byte[] {0x06, 0x1F},

                0
            },

            // Питер. табло LD45 (Обухово). Передача данных.
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
                        Header = "0xFF0xFF0xFF0x020x1BW",
                        Body = "САНКТ-ПЕТЕРБУРГ0x0912:340x0920x09САНКТПЕТЕРБУРГ0x0913:390x09 0x09ЛЮБАНЬ0x0912:220x0910x09КИРИШИ0x0912:550x09 0x09",
                        Footer = "0x03{CRCXor:X2_BorderExclude}0x1F",
                        Format = "Windows-1251",
                        MaxBodyLenght = 300
                    },
                    ResponseOption = new ResponseOption
                    {
                        ValidatorName = "EqualValidator",
                        EqualValidator = new EqualResponseValidatorOption
                        {
                            Body = "0x060x1F",
                            Format = "Windows-1251"
                        }
                    }
                },
                GetData4ViewRuleTest.InputTypesDefault,   
                //REQUEST
                "FFFFFF021B57D1C0CDCAD22DCFC5D2C5D0C1D3D0C30931323A3334093209D1C0CDCAD2CFC5D2C5D0C1D3D0C30931333A3339092009CBDEC1C0CDDC0931323A3232093109CAC8D0C8D8C80931323A35350920090336311F",
                "HEX",
                "0xFF0xFF0xFF0x020x1BWСАНКТ-ПЕТЕРБУРГ0x0912:340x0920x09САНКТПЕТЕРБУРГ0x0913:390x09 0x09ЛЮБАНЬ0x0912:220x0910x09КИРИШИ0x0912:550x09 0x090x03610x1F",   
                //RESPONSE
                new byte[] {0x06, 0x1F},

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
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.ListInputTypesDefault)?.ToArray();
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