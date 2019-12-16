using System.Collections.Generic;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class VidorViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Павелецкий Peron.vidor.P8 запрос 1
            new object[]
            {
                "5",                                           //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%000010320113%10$10$00$60$t2{Time:t}%000330940113%10$10$00$60$t3{Event}%000951920114%10$10$00$60$t1{StationsCut}%000011920183%10$10$00$60$t2 ",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "\u0002058D%000010320113%10$10$00$60$t216:18%000330940113%10$10$00$60$t3Отправление%000951920114%10$10$00$60$t1Москва-Питер%000011920183%10$10$00$60$t2 87\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u0002058D%000010320113%10$10$00$60$t216:18%000330940113%10$10$00$60$t3Отправление%000951920114%10$10$00$60$t1Москва-Питер%000011920183%10$10$00$60$t2 87\u0003",   //expectedStrRepresentBase
                3                                                                                                                                                                    //expectedCountInseartedData
            },
            //Павелецкий Peron.vidor.P8 запрос 2
            new object[]
            {
                "5",                                           //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%000011920304%10$10$00$60$t2{Note}%000011600483%10$10$00$60$t3Московское время%001611920483%10$10$00$60$t1",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "\u000205A9%000011920304%10$10$00$60$t2Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7%000011600483%10$10$00$60$t3Московское время%001611920483%10$10$00$60$t1F8\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u000205A9%000011920304%10$10$00$60$t2Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7%000011600483%10$10$00$60$t3Московское время%001611920483%10$10$00$60$t1F8\u0003",   //expectedStrRepresentBase
                1                                                                                                                                                                    //expectedCountInseartedData
            },
            //Павелецкий Prigorod.9Str
            new object[]
            {
                "10",                                          //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%00001032{(rowNumber*16):D3}3%10$12$00$60$t3{TDepart:t}%00033240{(rowNumber*16):D3}4%10$12$00$60$t3{StationArrival}%00241256{(rowNumber*16):D3}3%10$12$00$60$t1{PathNumber}%400012561451%000012561603%10$10$00$60$t2Московское время",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "\u00020A99%000010320163%10$12$00$60$t316:18%000332400164%10$12$00$60$t3Москва%002412560163%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское время66\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u00020A99%000010320163%10$12$00$60$t316:18%000332400164%10$12$00$60$t3Москва%002412560163%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское время66\u0003",   //expectedStrRepresentBase
               4                                                                                                                                                                 //expectedCountInseartedData
            },
            //Павелецкий Prigorod.9Str  - Batch=3
            new object[]
            {
                "10",                                          //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%00001032{(rowNumber*16):D3}3%10$12$00$60$t3{TDepart:t}%00033240{(rowNumber*16):D3}4%10$12$00$60$t3{StationArrival}%00241256{(rowNumber*16):D3}3%10$12$00$60$t1{PathNumber}%400012561451%000012561603%10$10$00$60$t2Московское время",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 500
                },
                GetData4ViewRuleTest.ListInputTypesDefault,                              //inputTypes
                "\u00020A1D0%000010320163%10$12$00$60$t316:18%000332400164%10$12$00$60$t3Москва%002412560163%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское время%000010320323%10$12$00$60$t317:18%000332400324%10$12$00$60$t3Лондон%002412560323%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское время%000010320483%10$12$00$60$t320:20%000332400484%10$12$00$60$t3Новосибирск%002412560483%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское времяDA\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u00020A1D0%000010320163%10$12$00$60$t316:18%000332400164%10$12$00$60$t3Москва%002412560163%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское время%000010320323%10$12$00$60$t317:18%000332400324%10$12$00$60$t3Лондон%002412560323%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское время%000010320483%10$12$00$60$t320:20%000332400484%10$12$00$60$t3Новосибирск%002412560483%10$12$00$60$t1 %400012561451%000012561603%10$10$00$60$t2Московское времяDA\u0003",   //expectedStrRepresentBase
                4                                                                                                                                                                 //expectedCountInseartedData
            },
            //Казанский Prib.Otpr.Double.6Str Data.Otpr Запрос 2
            new object[]
            {
                "3",                                          //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%00001021{(rowNumber*11+16):D3}4%10$00$60$t3$13{NumberOfTrain}%00025144{(rowNumber*11+16):D3}4%10$00$60$t3$13{StationArrival}%00168199{(rowNumber*11+16):D3}4%10$00$60$t3$13{TDepart:t}%00202233{(rowNumber*11+16):D3}4%10$00$60$t3$13%00202233{(rowNumber*11+16):D3}4%10$00$60$t3$13%00241254{(rowNumber*11+16):D3}4%10$00$60$t3$13{PathNumber}",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "\u000203B7%000010210274%10$00$60$t3$13456%000251440274%10$00$60$t3$13Москва%001681990274%10$00$60$t3$1316:18%002022330274%10$00$60$t3$13%002022330274%10$00$60$t3$13%002412540274%10$00$60$t3$13 9F\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u000203B7%000010210274%10$00$60$t3$13456%000251440274%10$00$60$t3$13Москва%001681990274%10$00$60$t3$1316:18%002022330274%10$00$60$t3$13%002022330274%10$00$60$t3$13%002412540274%10$00$60$t3$13 9F\u0003",   //expectedStrRepresentBase
                5                                                                                                                                                               //expectedCountInseartedData
            },

        };
        #endregion



        [Theory]
        [MemberData(nameof(Datas))]
        public void CreateStringRequestTest(
            string addressDevice,
            RequestOption requestOption,
            List<AdInputType> inputTypes,
            string expectedStrRepresent,
            string expectedStrRepresentFormat,
            string expectedStrRepresentBase,
            int expectedCountInseartedData)
        {
            ////Arrange
            //var viewRule = new ViewRule<AdInputType>(addressDevice, requestOption);

            ////Act
            //var requestTransfer = viewRule.CreateStringRequest(inputTypes, 0);

            ////Assert
            //requestTransfer.StrRepresent.Str.Should().Be(expectedStrRepresent);
            //requestTransfer.StrRepresent.Format.Should().Be(expectedStrRepresentFormat);
            //requestTransfer.StrRepresentBase.Str.Should().Be(expectedStrRepresentBase);
            //requestTransfer.StrRepresentBase.Format.Should().Be(requestOption.Format);
            //requestTransfer.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(inputTypes.Count);
            //foreach (var processedItem in requestTransfer.ProcessedItemsInBatch.ProcessedItems)
            //{
            //    processedItem.InseartedData.Count.Should().Be(expectedCountInseartedData);
            //}
        }

    }
}