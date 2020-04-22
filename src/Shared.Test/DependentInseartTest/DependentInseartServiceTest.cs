using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart;
using FluentAssertions;
using Xunit;

namespace Shared.Test.DependentInseartTest
{
    public class DependentInseartServiceTest
    {
        public const string Pattern = ViewRule<AdInputType>.Pattern;

        #region TheoryData
        public static IEnumerable<object[]> ExecuteInseartsDatas => new[]
        {
             //NumberOfCharacters calc test
             new object[]
             {
                 "",
                 "{NumberOfCharacters:X2}\\\"{NumberOfTrain}\\\"{NumberOfCharacters:X2}\\\"{TArrival:t}\\\"{NumberOfCharacters:X2}\\\"{PathNumber}\\\"",
                 "",
                 "Windows-1251",
                 new StringBuilder("{NumberOfCharacters:X2}\\\"42\\\"{NumberOfCharacters:X2}\\\"12:25\\\"{NumberOfCharacters:X2}\\\"5\\\""), 
                 "02420512:25015"
             },
            
             //Vidor
             new object[]
             {
                 "\u0002{AddressDevice:X2}{Nchar:X2}",
                 "%000010320113%10$10$00$60$t2{Time:t}%000330940113%10$10$00$60$t3{Event}%000951920114%10$10$00$60$t1{StationsCut}%000011920183%10$10$00$60$t2 ",
                 "{CRCXorInverse:X2}\u0003",
                 "Windows-1251",
                 new StringBuilder("\u000205{Nchar:X2}%000010320113%10$10$00$60$t215:25%000330940113%10$10$00$60$tПриб%000951920114%10$10$00$60$t1Москва-Питер%000011920183%10$10$00$60$t2 {CRCXorInverse:X2}\u0003"), 
                 "\u00020585%000010320113%10$10$00$60$t215:25%000330940113%10$10$00$60$tПриб%000951920114%10$10$00$60$t1Москва-Питер%000011920183%10$10$00$60$t2 31\u0003"
             },
            
            //InformSvyaz
             new object[]
             {
                 "0x{AddressDevice:X2}0x{NbyteFull:X2}",
                 "0x03{Hour:D2}0x3A{Minute:D2}",
                 "0x{CRCMod256:X2}",
                 "cp866",
                 new StringBuilder("0x080x{NbyteFull:X2}0x03100x3A450x{CRCMod256:X2}"), 
                 "0x080x090x03100x3A450x18"
             },
            
             //Ekrim (HEX only)
             new object[]
             {
                 "",
                 "041FFFFF1010FEFE051F",
                 "",
                 "HEX",
                 new StringBuilder("041FFFFF1010FEFE051F"), 
                 "041FFFFF1010FEFE051F"
             },
            
            //Ekrim (Data)
            new object[]
            {
                "0xFF0xFF0x020x1B0x57",
                "{Addition} {StationsCut}0x09{ExpectedTime:t}0x09{Note}0x09",
                "0x030x{CRCXor[0x02-0x03]:X2}0x1F",
                "Windows-1251",
                new StringBuilder("0xFF0xFF0x020x1B0x57Дополн. Хабаровск0x0918:000x09Со всеми остановками0x090x030x{CRCXor[0x02-0x03]:X2}0x1F"), 
                "0xFF0xFF0x020x1B0x57Дополн. Хабаровск0x0918:000x09Со всеми остановками0x090x030xA80x1F"
            },
            
            //Vidor (Habarovsk)
            new object[]
            {
                "\u0002{AddressDevice:X2}{Nchar:X2}",
                "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN%01000023{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0020001E%10{NumberOfCharacters:X2}01\\\"{NumberOfTrain}\\\"%01024030{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\" \\\"%01062086{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TArrival:t}\\\"%010870AB{(rowNumber*24-23):X3}{(rowNumber*24-13):X3}0000001E%10{NumberOfCharacters:X2}01\\\"{TDepart:t}\\\"",
                "{CRCXorInverse:X2}\u0003",
                "Windows-1251",
                new StringBuilder("\u000208{Nchar:X2}%3000125%010C60EF03B0470000001E%110406NNNNN%010000230010080020001E%10{NumberOfCharacters:X2}01\\\"856\\\"%010240300010080000001E%10{NumberOfCharacters:X2}01\\\" \\\"%01062086000080000001E%10{NumberOfCharacters:X2}01\\\"15:21\\\"%010870AB0010080000001E%10{NumberOfCharacters:X2}01\\\"20:50\\\"{CRCXorInverse:X2}\u0003"), 
                "\u000208B0%3000125%010C60EF03B0470000001E%110406NNNNN%010000230010080020001E%100301856%010240300010080000001E%100101 %01062086000080000001E%10050115:21%010870AB0010080000001E%10050120:5080\u0003"
            },
        };
        #endregion
        [Theory]
        [MemberData(nameof(ExecuteInseartsDatas))]
        public void Normal_ExecuteInsearts_Test(string header, string body, string footer, string format, StringBuilder strAfterIndepIns, string expectedStr)
        {
            //Arrange
            string strInit = header+body+footer;
            var depServ = DependentInseartsServiceFactory.Create(strInit, Pattern);
            
            //Act
            var (_, isFailure, value) = depServ.ExecuteInsearts(strAfterIndepIns, format);
            
            //Assert
            isFailure.Should().BeFalse();
            value.ToString().Should().Be(expectedStr);
        }
        
        
        [Fact]
        public void ServiceFactory_Exception_crcModel_NotFound_For_Nchar_Handler_Test()
        {
            //Arrange
            var header = "\u0002{AddressDevice:X2}{Nchar:X2}";
            var body= "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN%01000023{(rowNumber*24-23){NumberOfCharacters01\\\"78\\\"";
            var footer= "{CRCXorInverse:X2\u0003";
            var strInit = header+body+footer;
            
            //Act & Asert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => DependentInseartsServiceFactory.Create(strInit, Pattern));
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Contain("Value cannot be null. (Parameter 'crcModel')");
        }
        
        
        [Fact]
        public void Empty_String_Test()
        {
            //Arrange
            string header = "";
            string body= "";
            string footer= "";
            string strInit = header+body+footer;
            var depServ = DependentInseartsServiceFactory.Create(strInit, Pattern);
            
            //Act
            StringBuilder strAfterIndepIns= new StringBuilder(strInit);
            string format= "Windows-1251";
            var result= depServ.ExecuteInsearts(strAfterIndepIns, format);
            
            //Assert
            string expectedStr= "";
            result.IsFailure.Should().BeFalse();
            result.Value.ToString().Should().Be(expectedStr);
        }
        
        
        [Fact]
        public void Trash_String_Test()
        {
            //Arrange
            var header = "\u0002{AddressDevice:X2}{Nchar:X2}";
            var body= "%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN%01000023{(rowNumber*24-23){NumberOfCharacters01\\\"78\\\"{CRCXorInverse";
            var footer= "{CRCXorInverse:X2}\u0003";
            string strInit = header+body+footer;
            var depServ = DependentInseartsServiceFactory.Create(strInit, Pattern);
            
            //Act
            StringBuilder strAfterIndepIns= new StringBuilder(strInit);
            string format= "Windows-1251";
            var result= depServ.ExecuteInsearts(strAfterIndepIns, format);
            
            //Assert
            string expectedStr= "\u0002{AddressDevice:X2}79%30{SyncTInSec:X5}%010C60EF03B0470000001E%110406NNNNN%01000023{(rowNumber*24-23){NumberOfCharacters01\\\"78\\\"{CRCXorInverseFD\u0003";
            result.IsFailure.Should().BeFalse();
            result.Value.ToString().Should().Be(expectedStr);
        }
        
        
        [Fact]
        public void NumberOfChar_WithOut_quotes_String_Test()  //Если для NumberOfCharacters не указанны \\\" \\\", то размер считается равен 0 (БЕЗ ОШИБОК)
        {
            //Arrange
            var header = "\u0002{AddressDevice:X2}{Nchar:X2}";
            var body= "%30{SyncTInSec:X5}%010C60EF03B0470000{NumberOfCharacters:X2}0178";
            var footer= "{CRCXorInverse:X2}\u0003";
            string strInit = header+body+footer;
            var depServ = DependentInseartsServiceFactory.Create(strInit, Pattern);
            
            //Act
            StringBuilder strAfterIndepIns= new StringBuilder("\u000205{Nchar:X2}%3000025%010C60EF03B0470000{NumberOfCharacters:X2}0178{CRCXorInverse:X2}\u0003");
            string format= "Windows-1251";
            var result= depServ.ExecuteInsearts(strAfterIndepIns, format);
            
            //Assert
            string expectedStr= "\u00020521%3000025%010C60EF03B0470000000178C6\u0003";
            result.IsFailure.Should().BeFalse();
            result.Value.ToString().Should().Be(expectedStr);
        }
    }
}