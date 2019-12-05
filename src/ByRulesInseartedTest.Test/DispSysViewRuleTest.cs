using System.Collections.Generic;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class DispSysViewRuleTest
    {
        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            // Хабаровск Perehod.P3.P5 запрос 1
            new object[]
            {
                "5",                                           //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%30%010C60EF03B0470000001E%110406NNNNN%01000023{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0020001E%10{NumberOfCharacters:X2}01\\\"{NumberOfTrain}\\\"%01024030{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\" \\\"%01062086{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TArrival:t}\\\"%010870AB{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TDepart:t}\\\"",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "\u000205AC%30%010C60EF03B0470000001E%110406NNNNN%0100002300100B0020001E%100301456%0102403000100B0000001E%100101 %0106208600100B0000001E%10050115:25%010870AB00100B0000001E%10050116:18FB\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u000205AC%30%010C60EF03B0470000001E%110406NNNNN%0100002300100B0020001E%100301456%0102403000100B0000001E%100101 %0106208600100B0000001E%10050115:25%010870AB00100B0000001E%10050116:18FB\u0003",   //expectedStrRepresentBase
                5                                                                                                                                                                    //expectedCountInseartedData
            },
            // Хабаровск Perehod.P3.P5 запрос 2
            new object[]
            {
                "5",                                           //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%010000F0{(rowNumber*24-11):X3}{(rowNumber*24-1):X3}0024001E%10{NumberOfCharacters:X2}01\\\"{Addition} {StationDeparture}-{StationArrival}\\\"%010AC0D5{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TypeAlias}\\\"%010D60F0{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0020001E%10{NumberOfCharacters:X2}01\\\"{DelayTime:Min}\\\"",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "\u0002056A%010000F000D0170024001E%100E01  Питер-Москва%010AC0D500100B0000001E%100101 %010D60F000100B0020001E%100101 2C\u0003",   //expectedStrRepresent
                "Windows-1251",                                                                                                                                                      //expectedStrRepresentFormat
                "\u0002056A%010000F000D0170024001E%100E01  Питер-Москва%010AC0D500100B0000001E%100101 %010D60F000100B0020001E%100101 2C\u0003",   //expectedStrRepresentBase
                9                                                                                                                                                                    //expectedCountInseartedData
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