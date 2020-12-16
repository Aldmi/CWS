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
    public class VidorViewRuleTest : BaseViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Павелецкий Peron.vidor.P8 запрос 1
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    UnitOfSendings = new List<UnitOfSendingOption> {
                            new UnitOfSendingOption {
                                RequestOption = new RequestOption
                                {
                                    Header = "\u0002{AddressDevice:X2}{Nchar:X2_BorderRightBeforeCrc}",
                                    Body = "%000010320113%10$10$00$60$t2{Time:t}%000330940113%10$10$00$60$t3{Event}%000951920114%10$10$00$60$t1{StationsCut}%000011920183%10$10$00$60$t2 ",
                                    Footer = "{CRCXorInverse:X2_BorderLeft}\u0003",
                                    Format = "Windows-1251",
                                    MaxBodyLenght = 245
                                },
                                ResponseOption = new ResponseOption
                                {
                                    ValidatorName = "EqualValidator",
                                    EqualValidator = new EqualResponseValidatorOption
                                    {
                                        Body = "0230323030464403",
                                        Format ="HEX"
                                    }
                                }
                            }
                        }
                },

                //REQUEST
                GetData4ViewRuleTest.InputTypesDefault,
                "\u00020589%000010320113%10$10$00$60$t216:18%000330940113%10$10$00$60$t3Транзит%000951920114%10$10$00$60$t1Питер-Москва%000011920183%10$10$00$60$t2 E7\u0003",
                "Windows-1251",
                "\u00020589%000010320113%10$10$00$60$t216:18%000330940113%10$10$00$60$t3Транзит%000951920114%10$10$00$60$t1Питер-Москва%000011920183%10$10$00$60$t2 E7\u0003",   
                //RESPONSE
                new byte[]{0x02, 0x30, 0x32, 0x30, 0x30, 0x46, 0x44, 0x03},

                3
            },

            //Павелецкий Peron.vidor.P8 запрос 2
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    UnitOfSendings = new List<UnitOfSendingOption> {
                            new UnitOfSendingOption {
                                RequestOption = new RequestOption
                                {
                                    Header = "\u0002{AddressDevice:X2}{Nchar:X2_BorderRightBeforeCrc}",
                                    Body = "%000011920304%10$10$00$60$t2{Note}%000011600483%10$10$00$60$t3Московское время%001611920483%10$10$00$60$t1",
                                    Footer = "{CRCXorInverse:X2_BorderLeft}\u0003",
                                    Format = "Windows-1251",
                                    MaxBodyLenght = 245
                                },
                                ResponseOption = new ResponseOption
                                {
                                    ValidatorName = "EqualValidator",
                                    EqualValidator = new EqualResponseValidatorOption
                                    {
                                        Body = "0230323030464403",
                                        Format ="HEX"
                                    }
                                }
                            }
                        }
                },

                //REQUEST
                GetData4ViewRuleTest.InputTypesDefault,
                "\u000205A9%000011920304%10$10$00$60$t2Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7%000011600483%10$10$00$60$t3Московское время%001611920483%10$10$00$60$t1F8\u0003",
                "Windows-1251",
                "\u000205A9%000011920304%10$10$00$60$t2Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7%000011600483%10$10$00$60$t3Московское время%001611920483%10$10$00$60$t1F8\u0003",  
                //RESPONSE
                new byte[]{0x02, 0x30, 0x32, 0x30, 0x30, 0x46, 0x44, 0x03},

                1
            },

            //Павелецкий Prigorod.9Str
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    UnitOfSendings = new List<UnitOfSendingOption> {
                            new UnitOfSendingOption {
                                RequestOption = new RequestOption
                                {
                                    Header = "\u0002{AddressDevice:X2}{Nchar:X2_BorderRightBeforeCrc}",
                                    Body = "%00001032{MATH(rowNumber*16):D3}3%10$12$00$60$t3{TDepart:t}%00033240{MATH(rowNumber*16):D3}4%10$12$00$60$t3{StationArrival}%00241256{MATH(rowNumber*16):D3}3%10$12$00$60$t1{PathNumber}%400012561451%000012561603%10$10$00$60$t2Московское время",
                                    Footer = "{CRCXorInverse:X2_BorderLeft}\u0003",
                                    Format = "Windows-1251",
                                    MaxBodyLenght = 245
                                },
                                ResponseOption = new ResponseOption
                                {
                                    ValidatorName = "EqualValidator",
                                    EqualValidator = new EqualResponseValidatorOption
                                    {
                                        Body = "0230323030464403",
                                        Format ="HEX"
                                    }
                                }
                            }
                        }
                },

                //REQUEST
                GetData4ViewRuleTest.InputTypesDefault,
                "\u00020599%000010320163%10$12$00$60$t316:18%000332400164%10$12$00$60$t3Москва%002412560163%10$12$00$60$t15%400012561451%000012561603%10$10$00$60$t2Московское время07\u0003",
                "Windows-1251",
                "\u00020599%000010320163%10$12$00$60$t316:18%000332400164%10$12$00$60$t3Москва%002412560163%10$12$00$60$t15%400012561451%000012561603%10$10$00$60$t2Московское время07\u0003",   
                //RESPONSE
                new byte[]{0x02, 0x30, 0x32, 0x30, 0x30, 0x46, 0x44, 0x03},

               3
            },
            //Казанский Prib.Otpr.Double.6Str Data.Otpr Запрос 2
            new object[]
            {
                "5",
                new ViewRuleOption()
                {
                    Id = 1,
                    StartPosition = 0,
                    Count = 1,
                    BatchSize = 1,
                    UnitOfSendings = new List<UnitOfSendingOption> {
                            new UnitOfSendingOption {
                                RequestOption = new RequestOption
                                {
                                    Header = "\u0002{AddressDevice:X2}{Nchar:X2_BorderRightBeforeCrc}",
                                    Body = "%00001021{MATH(rowNumber*11+16):D3}4%10$00$60$t3$13{NumberOfTrain}%00025144{MATH(rowNumber*11+16):D3}4%10$00$60$t3$13{StationArrival}%00168199{MATH(rowNumber*11+16):D3}4%10$00$60$t3$13{TDepart:t}%00202233{MATH(rowNumber*11+16):D3}4%10$00$60$t3$13%00202233{MATH(rowNumber*11+16):D3}4%10$00$60$t3$13%00241254{MATH(rowNumber*11+16):D3}4%10$00$60$t3$13{PathNumber}",
                                    Footer = "{CRCXorInverse:X2_BorderLeft}\u0003",
                                    Format = "Windows-1251",
                                    MaxBodyLenght = 245
                                },
                                ResponseOption = new ResponseOption
                                {
                                    ValidatorName = "EqualValidator",
                                    EqualValidator = new EqualResponseValidatorOption
                                    {
                                        Body = "0230323030464403",
                                        Format ="HEX"
                                    }
                                }
                            }
                        }
                },

                //REQUEST
                GetData4ViewRuleTest.InputTypesDefault,
                "\u000205B7%000010210274%10$00$60$t3$13456%000251440274%10$00$60$t3$13Москва%001681990274%10$00$60$t3$1316:18%002022330274%10$00$60$t3$13%002022330274%10$00$60$t3$13%002412540274%10$00$60$t3$1358C\u0003",   //expectedStrRepresent
                "Windows-1251",
                "\u000205B7%000010210274%10$00$60$t3$13456%000251440274%10$00$60$t3$13Москва%001681990274%10$00$60$t3$1316:18%002022330274%10$00$60$t3$13%002022330274%10$00$60$t3$13%002412540274%10$00$60$t3$1358C\u0003",
                //RESPONSE
                new byte[]{0x02, 0x30, 0x32, 0x30, 0x30, 0x46, 0x44, 0x03},

                4
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
            var viewRule = ViewRule<AdInputType>.Create(option, addressDevice,  InTypeIndependentInsertsHandlerFactory, StringInsertModelExtDictionary, InlineInseartService, Logger);

            //Act
            var requestTransfers = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArrayAsync().GetAwaiter().GetResult();
            var (isSuccess, _, providerTransfer, error) = requestTransfers.FirstOrDefault();

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