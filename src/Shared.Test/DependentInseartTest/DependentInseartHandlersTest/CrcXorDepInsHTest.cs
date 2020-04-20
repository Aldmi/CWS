using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Xunit;

namespace Shared.Test.DependentInseartTest.DependentInseartHandlersTest
{
    public class CrcXorDepInsHTest
    {
        private readonly CrcXorDepInsH _handler;
        public CrcXorDepInsHTest()
        {
            var requiredModel = new StringInsertModel("{CRCXor[0x02-0x03]:X2}", "CRCXor", "[0x02-0x03]", ":X2");
            _handler = new CrcXorDepInsH(requiredModel);
        }
        
        
        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x030x{CRCXor[0x02-0x03]:X2}0x1F");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x030xBA0x1F");
        }
        
        
        [Fact]
        public void Replace_FirstOccurence_Crc()
        {
            //Arrange
            var sb = new StringBuilder("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x{CRCXor[0x02-0x03]:X2}0x030x{CRCXor[0x02-0x03]:X2}0x1F");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250xDA0x030x{CRCXor[0x02-0x03]:X2}0x1F");
        }
        
        
        [Fact]
        public void String_With_Trash()
        {
            //Arrange
            var sb = new StringBuilder("0xFF0xFF0x020x1B0x57{AddressDevice:X2}{Nbyte:X2}дополнение0x09Москва-Питер0x0915:250x{CRCXor[0x02-0x03]:X20x030x{CRCXor[0x02-0x03]:X2}0x1F");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0xFF0xFF0x020x1B0x57{AddressDevice:X2}{Nbyte:X2}дополнение0x09Москва-Питер0x0915:250x{CRCXor[0x02-0x03]:X20x030xF00x1F");
        }
        
        
        [Fact]
        public void Calc_Crc_WithOut_CrcOptions_Test()
        {
            //Arrange
            var requiredModel = new StringInsertModel("{CRCXor:X2}", "CRCXor", "", ":X2");
            var handler = new CrcXorDepInsH(requiredModel);
            var sb = new StringBuilder("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x030x{CRCXor:X2}0x1F");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x030xBA0x1F");
        }
        
        [Fact]
        public void Calc_Crc_WithOut_CrcOptions_WithOut_HexInStr_Test()
        {
            //Arrange
            var requiredModel = new StringInsertModel("{CRCXor:X2}", "CRCXor", "", ":X2");
            var handler = new CrcXorDepInsH(requiredModel);
            var sb = new StringBuilder("\u000201BA%00001021дополнение4%10$00$60$t3$13Москва-Питер%10$00$615:254%10$00$60$t3$13{CRCXor:X2}\u0003");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u000201BA%00001021дополнение4%10$00$60$t3$13Москва-Питер%10$00$615:254%10$00$60$t3$13D0\u0003");
        }
    }
}