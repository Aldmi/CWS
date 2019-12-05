using System.Collections.Generic;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class InformSvayzViewRuleTest
    {

        #region TheoryData
        public static IEnumerable<object[]> VidorDatas => new[]
        {
            // Павелецкий Peron.P1 Data2
            new object[]
            {
                "5",                                           //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "0x{AddressDevice:X2}0x{NbyteFull:X2}",
                    Body = "0x03^{NumberOfTrain}^ {StationsCut}^{TArrival:t}^{TDepart:t}",
                    Footer = "0x{CRCMod256:X2}",
                    Format = "cp866",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "0522035E3435365E208CAEE1AAA2A02D8FA8E2A5E05E31353A32355E31363A313844",   //expectedStrRepresent
                "HEX",                                                                                                                                                      //expectedStrRepresentFormat
                "0x050x220x03^456^ Москва-Питер^15:25^16:180x44",   //expectedStrRepresentBase
                4                                                                                                                                                                    //expectedCountInseartedData
            },
            // Павелецкий Peron.P1 Data3
            new object[]
            {
                "5",                                           //addressDevice
                new RequestOption                              //requestOption
                {
                    Header = "0x{AddressDevice:X2}0x{NbyteFull:X2}",
                    Body = "0x03{Note}",
                    Footer = "0x{CRCMod256:X2}",
                    Format = "cp866",
                    MaxBodyLenght = 245
                },
                GetData4ViewRuleTest.InputTypesDefault,                              //inputTypes
                "05490391E2A0ADE6A8EF20312C91E2A0ADE6A8EF20322C91E2A0ADE6A8EF20332C91E2A0ADE6A8EF20342C91E2A0ADE6A8EF20352C91E2A0ADE6A8EF20362C91E2A0ADE6A8EF203750",       //expectedStrRepresent
                "HEX",                                                                                                                                                      //expectedStrRepresentFormat
                "0x050x490x03Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 70x50",                                                                    //expectedStrRepresentBase
                1                                                                                                                                                                    //expectedCountInseartedData
            },
        };
        #endregion



        [Theory]
        [MemberData(nameof(VidorDatas))]
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