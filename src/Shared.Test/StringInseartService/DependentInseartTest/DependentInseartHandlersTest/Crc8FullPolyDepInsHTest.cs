using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Shared.Test.StringInseartService.Datas;
using Xunit;

namespace Shared.Test.StringInseartService.DependentInseartTest.DependentInseartHandlersTest
{
    public class Crc8FullPolyDepInsHTest
    {
        private readonly Crc8FullPolyDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        public Crc8FullPolyDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{CRC8FullPoly:X2_Border_StartOnly}", _extDict).First();
            _handler = new Crc8FullPolyDepInsH(requiredModel);
        }


        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x160x480x{CRC8FullPoly:X2_Border_StartOnly}0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x160x480xCF0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
        }


        //[Fact]
        //public void Replace_FirstOccurence_Crc()
        //{
        //    //Arrange
        //    var sb = new StringBuilder("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x030x{CRCXor:X2_Border}0x030x{CRCXor:X2_Border}0x1F");
        //    var format = "cp866";
        //    //Act
        //    var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
        //    //Assert
        //    isSuccess.Should().BeTrue();
        //    value.ToString().Should().Be("0xFF0xFF0x020x1B0x57дополнение0x09Москва-Питер0x0915:250x030xBA0x030x{CRCXor:X2_Border}0x1F");
        //}


        //[Fact]
        //public void String_With_Trash()
        //{
        //    //Arrange
        //    var sb = new StringBuilder("0xFF0xFF0x020x1B0x57{AddressDevice:X2}{Nbyte:X2}дополнение0x09Москва-Питер0x0915:250x{CRCXor:X2_Border0x030x{CRCXor:X2_Border}0x1F");
        //    var format = "cp866";
        //    //Act
        //    var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
        //    //Assert
        //    isSuccess.Should().BeTrue();
        //    value.ToString().Should().Be("0xFF0xFF0x020x1B0x57{AddressDevice:X2}{Nbyte:X2}дополнение0x09Москва-Питер0x0915:250x{CRCXor:X2_Border0x030xA70x1F");
        //}


        //[Fact]
        //public void Calc_Crc_WithOut_CrcOptions_WithOut_HexInStr_Test()
        //{
        //    //Arrange
        //    var requiredModel = StringInsertModelFactory.CreateList("{CRCXor:X2}", _extDict).First();
        //    var handler = new CrcXorDepInsH(requiredModel);
        //    var sb = new StringBuilder("\u000201BA%00001021дополнение4%10$00$60$t3$13Москва-Питер%10$00$615:254%10$00$60$t3$13{CRCXor:X2}\u0003");
        //    var format = "Windows-1251";
        //    //Act
        //    var (isSuccess, _, value) = handler.CalcInsert(sb, format);
        //    //Assert
        //    isSuccess.Should().BeTrue();
        //    value.ToString().Should().Be("\u000201BA%00001021дополнение4%10$00$60$t3$13Москва-Питер%10$00$615:254%10$00$60$t3$13D0\u0003");
        //}


        //[Fact]
        //public void Border_Not_Found_In_Str()
        //{
        //    //Arrange
        //    var sb = new StringBuilder("0xFF0xAA0x{CRCXor:X2_Border}0x1F");
        //    var format = "cp866";
        //    //Act
        //    var (isSuccess, _, value, error) = _handler.CalcInsert(sb, format);
        //    //Assert
        //    isSuccess.Should().BeFalse();
        //    error.Should().Be("Not Found startCh= 0x02");
        //}


        //[Fact]
        //public void Crc_WithOut_StringInseartExt()
        //{
        //    //Arrange
        //    var requiredModel = StringInsertModelFactory.CreateList("{CRCXor}", _extDict).First();
        //    var handler = new CrcXorDepInsH(requiredModel);
        //    var sb = new StringBuilder("0xFF0xAA0x{CRCXor}0x1F");
        //    var format = "cp866";
        //    //Act
        //    var (isSuccess, _, value) = handler.CalcInsert(sb, format);
        //    //Assert
        //    isSuccess.Should().BeTrue();
        //    value.ToString().Should().Be("0xFF0xAA0x850x1F");
        //}
    }
}